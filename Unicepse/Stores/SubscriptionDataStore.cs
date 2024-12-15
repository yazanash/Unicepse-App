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
using Unicepse.Stores.ApiDataStores;

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

        private readonly IDataService<Subscription> _subscriptionDataService;
        private readonly IGetPlayerTransactionService<Subscription> _getPlayerTransactionService;
        private readonly IActiveTransactionService<Subscription> _activeTransactionService;
        private readonly IApiDataStore<Subscription>  _apiDataStore;
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
        public SubscriptionDataStore(IDataService<Subscription> subscriptionDataService,  ILogger<SubscriptionDataStore> logger, IGetPlayerTransactionService<Subscription> getPlayerTransactionService, IActiveTransactionService<Subscription> activeTransactionService, IApiDataStore<Subscription> apiDataStore)
        {
            _subscriptionDataService = subscriptionDataService;
            _subscriptions = new List<Subscription>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _logger = logger;
            _getPlayerTransactionService = getPlayerTransactionService;
            _activeTransactionService = activeTransactionService;
            _apiDataStore = apiDataStore;
        }

        public async Task Add(Subscription entity)
        {
            _logger.LogInformation(LogFlag + "add subscription");
            entity.DataStatus = DataStatus.ToCreate;
            await _subscriptionDataService.Create(entity);
            await _apiDataStore.Create(entity);
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
            IEnumerable<Subscription> subscriptions = await _getPlayerTransactionService.GetAll(player);
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
            IEnumerable<Subscription> subscriptions = await _activeTransactionService.GetAll();
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
            await _apiDataStore.Update(entity);

          

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
            entity.EndDate = stopDate;
            entity.IsStopped = true;
            await _subscriptionDataService.Update(entity);


            await _apiDataStore.Update(entity);


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
      
    }
}
