using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Subscription;
using Unicepse.Entityframework.Services;
using Unicepse.ViewModels.SportsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Services;
using Unicepse.Core.Common;
using Unicepse.BackgroundServices;
using Microsoft.Extensions.Logging;
using Unicepse.Core.Services;

namespace Unicepse.Stores
{
    public class SubscriptionDataStore : IDataStore<Subscription>
    {
        public event Action<Subscription>? Created;
        public event Action? Loaded;
        public event Action<Subscription>? Updated;
        public event Action<int>? Deleted;

        string LogFlag = "[Subscriptions] ";
        private readonly ILogger<SubscriptionDataStore> _logger;

        private readonly ISubscriptionDataService _subscriptionDataService;
        private readonly SubscriptionApiDataService _subscriptionApiDataService;
        private readonly List<Subscription> _subscriptions;
        private readonly Lazy<Task> _initializeLazy;

        public IEnumerable<Subscription> Subscriptions => _subscriptions;

        private Sport? _selectedSport;
        public Sport? SelectedSport
        {
            get
            {
                return _selectedSport;
            }
            set
            {
                _selectedSport = value;

                StateChanged?.Invoke(SelectedSport);
            }
        }

        public event Action<Sport?>? StateChanged;
        private Employee? _selectedTrainer;
        public Employee? SelectedTrainer
        {
            get
            {
                return _selectedTrainer;
            }
            set
            {
                _selectedTrainer = value;
                TrainerChanged?.Invoke(SelectedTrainer);
            }
        }

        public event Action<Employee?>? TrainerChanged;


        private Subscription? _selectedSubscription;
        public Subscription? SelectedSubscription
        {
            get
            {
                return _selectedSubscription;
            }
            set
            {
                _selectedSubscription = value;
                SubscriptionChanged?.Invoke(SelectedSubscription);
            }
        }

        public event Action<Subscription?>? SubscriptionChanged;
        public SubscriptionDataStore(ISubscriptionDataService subscriptionDataService, SubscriptionApiDataService subscriptionApiDataService, ILogger<SubscriptionDataStore> logger)
        {
            _subscriptionDataService = subscriptionDataService;
            _subscriptions = new List<Subscription>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _subscriptionApiDataService = subscriptionApiDataService;
            _logger = logger;
        }

        public async Task Add(Subscription entity)
        {
            _logger.LogInformation(LogFlag + "add subscription");
            entity.DataStatus = DataStatus.ToCreate;
            await _subscriptionDataService.Create(entity);
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "add attendance to api");
                    int status = await _subscriptionApiDataService.Create(entity);
                    if (status == 201 || status == 409)
                    {
                        _logger.LogInformation(LogFlag + "subscription synced successfully with code {0}", status.ToString());
                        entity.DataStatus = DataStatus.Synced;
                        await _subscriptionDataService.UpdateDataStatus(entity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "subscription synced failed with error {0}", ex.Message);
                }

            }
            _subscriptions.Add(entity);
            Created?.Invoke(entity);
            SelectedTrainer = null;
            SelectedSport = null;
        }

        public async Task Delete(int entity_id)
        {
            _logger.LogInformation(LogFlag + "delete subscription");
            bool deleted = await _subscriptionDataService.Delete(entity_id);
            int currentIndex = _subscriptions.FindIndex(y => y.Id == entity_id);
            _subscriptions.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll(Player player)
        {
            _logger.LogInformation(LogFlag + "get all subscription");
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetAll(player);
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            Loaded?.Invoke();
        }
        public async Task GetAll(Employee trainer)
        {
            _logger.LogInformation(LogFlag + "get all trainer subscription");
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetAll(trainer);
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            Loaded?.Invoke();
        }

        public async Task GetAll(Sport sport, DateTime date)
        {
            _logger.LogInformation(LogFlag + "get all subscription for sport in date {0}", date);
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetAll(sport, date);
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            Loaded?.Invoke();
        }


        public async Task GetAll(Employee trainer, DateTime date)
        {
            _logger.LogInformation(LogFlag + "get all subscription for trainer in date {0}", date);
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetAll(trainer, date);
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            Loaded?.Invoke();
        }

        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all subscription");
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetAll();
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            Loaded?.Invoke();
        }
        public async Task GetAllActive()
        {
            _logger.LogInformation(LogFlag + "get all active subscription");
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetAllActive();
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            Loaded?.Invoke();
        }
        public async Task Initialize()
        {
            _logger.LogInformation(LogFlag + "get all subscription from init");
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetAll();
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
        }

        public async Task Update(Subscription entity)
        {
            _logger.LogInformation(LogFlag + "update Subscription");
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;

            await _subscriptionDataService.Update(entity);


            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "add Subscription to api");
                    int status = await _subscriptionApiDataService.Update(entity);
                    if (status == 200)
                    {
                        _logger.LogInformation(LogFlag + "Subscription synced successfully with code {0}", status.ToString());
                        entity.DataStatus = DataStatus.Synced;
                        await _subscriptionDataService.UpdateDataStatus(entity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "Subscription synced failed with error {0}", ex.Message);
                }

            }

            int currentIndex = _subscriptions.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _subscriptions[currentIndex] = entity;
            }
            else
            {
                _subscriptions.Add(entity);
            }
            Updated?.Invoke(entity);
            SelectedTrainer = null;
            SelectedSport = null;
        }
        public async Task Stop(Subscription entity, DateTime stopDate)
        {
            _logger.LogInformation(LogFlag + "stop subscription");
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;

            await _subscriptionDataService.Stop(entity, stopDate);


            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "add subscription to api");
                    int status = await _subscriptionApiDataService.Update(entity);
                    if (status == 200)
                    {
                        _logger.LogInformation(LogFlag + "subscription synced successfully with code {0}", status.ToString());
                        entity.DataStatus = DataStatus.Synced;
                        await _subscriptionDataService.UpdateDataStatus(entity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "subscription synced failed with error {0}", ex.Message);
                }
            }


            int currentIndex = _subscriptions.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _subscriptions[currentIndex] = entity;
            }
            else
            {
                _subscriptions.Add(entity);
            }
            Updated?.Invoke(entity);
        }
        public async Task MoveToNewTrainer(Subscription entity, Employee trainer, DateTime movedate)
        {
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;

            await _subscriptionDataService.MoveToNewTrainer(entity, trainer, movedate);


            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                try
                {
                    int status = await _subscriptionApiDataService.Update(entity);
                    if (status == 200)
                    {
                        entity.DataStatus = DataStatus.Synced;
                        await _subscriptionDataService.UpdateDataStatus(entity);
                    }
                }
                catch { }


            }
            int currentIndex = _subscriptions.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _subscriptions[currentIndex] = entity;
            }
            else
            {
                _subscriptions.Add(entity);
            }
            Updated?.Invoke(entity);
            SelectedTrainer = null;
            SelectedSport = null;
        }
        public async Task SyncSubscriptionsToCreate()
        {
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetByDataStatus(DataStatus.ToCreate);
            foreach (Subscription subscription in subscriptions)
            {
                _logger.LogInformation(LogFlag + "create subscription to api");
                int status = await _subscriptionApiDataService.Create(subscription);
                if (status == 201 || status == 409)
                {
                    _logger.LogInformation(LogFlag + "subscription synced successfully with code {0}", status.ToString());
                    subscription.DataStatus = DataStatus.Synced;
                    await _subscriptionDataService.UpdateDataStatus(subscription);
                }
                else
                {
                    _logger.LogWarning(LogFlag + "subscription synced failed with code {0}", status.ToString());
                }

            }
        }

        public async Task SyncSubscriptionsToUpdate()
        {
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetByDataStatus(DataStatus.ToUpdate);
            foreach (Subscription subscription in subscriptions)
            {
                _logger.LogInformation(LogFlag + "update subscription to api");
                int status = await _subscriptionApiDataService.Update(subscription);
                if (status == 200)
                {
                    _logger.LogInformation(LogFlag + "subscription synced successfully with code {0}", status.ToString());
                    subscription.DataStatus = DataStatus.Synced;
                    await _subscriptionDataService.UpdateDataStatus(subscription);
                }
                else
                {
                    _logger.LogWarning(LogFlag + "subscription synced failed with code {0}", status.ToString());
                }


            }
        }
    }
}
