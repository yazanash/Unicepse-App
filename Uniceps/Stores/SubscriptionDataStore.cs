using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.BackgroundServices;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services;
using Uniceps.MessengerSystem;
using Uniceps.MessengerSystem.Events;
using Uniceps.Models;
using Uniceps.Stores.ApiDataStores;
using Uniceps.SystemServices;
using Uniceps.ViewModels.SportsViewModels;

namespace Uniceps.Stores
{
    public class SubscriptionDataStore : IDataStore<Subscription>
    {
        public event Action<Subscription>? Created;
        public event Action? Loaded;
        public event Action<Subscription>? Updated;
        public event Action<int>? Deleted;
        public event Action? AllLoaded;

        string LogFlag = "[Subscriptions] ";
        private readonly ILogger<SubscriptionDataStore> _logger;

        private readonly IDataService<Subscription> _subscriptionDataService;
        private readonly IGetPlayerTransactionService<Subscription> _getPlayerTransactionService;
        private readonly ISubscriptionRenewService _subscriptionRenewService;
        private readonly List<Subscription> _subscriptions;
        private readonly Lazy<Task> _initializeLazy;
        public IEnumerable<Subscription> Subscriptions => _subscriptions;

        private readonly List<Subscription> _allSubscriptions;
        public IEnumerable<Subscription> AllSubscriptions => _allSubscriptions;

        public SubscriptionDataStore(IDataService<Subscription> subscriptionDataService, ILogger<SubscriptionDataStore> logger, IGetPlayerTransactionService<Subscription> getPlayerTransactionService, ISubscriptionRenewService subscriptionRenewService)
        {
            _subscriptionDataService = subscriptionDataService;
            _subscriptions = new List<Subscription>();
            _allSubscriptions = new List<Subscription>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _logger = logger;
            _getPlayerTransactionService = getPlayerTransactionService;
            _subscriptionRenewService = subscriptionRenewService;

            Messenger.Default.Register<PaymentCreated>(this, OnPaymentCreated);
            Messenger.Default.Register<PaymentDeleted>(this, OnPaymentDeleted);

            Messenger.Default.Register<EntityUpdated<Player>>(this, OnPlayerUpdated);
            Messenger.Default.Register<EntityUpdated<Sport>>(this, OnSportUpdated);
            Messenger.Default.Register<EntityUpdated<Employee>>(this, OnTrainerUpdated);
        }

        private void OnTrainerUpdated(EntityUpdated<Employee> updated)
        {
            var subsToUpdate = _allSubscriptions.Where(x => x.TrainerId == updated.Entity.Id).ToList();
            foreach (var sub in subsToUpdate)
            {
                sub.TrainerName = updated.Entity.FullName;
                RefreshUi(sub);
            }
        }

        private void OnSportUpdated(EntityUpdated<Sport> updated)
        {
            var subsToUpdate = _allSubscriptions.Where(x => x.SportId == updated.Entity.Id).ToList();
            foreach (var sub in subsToUpdate)
            {
                sub.SportName = updated.Entity.Name;
                RefreshUi(sub);
            }
        }

        private void OnPlayerUpdated(EntityUpdated<Player> updated)
        {
            var subsToUpdate = _allSubscriptions.Where(x => x.PlayerId == updated.Entity.Id).ToList();
            foreach (var sub in subsToUpdate)
            {
                sub.PlayerName = updated.Entity.FullName;
                sub.PlayerPhone = updated.Entity.Phone;
                RefreshUi(sub);
            }
        }

        private void OnPaymentDeleted(PaymentDeleted deleted)
        {
            Subscription? currentSubscription = _subscriptions.FirstOrDefault(y => y.Id == deleted.SubscriptionId);
            if (currentSubscription != null)
            {
                currentSubscription.TotalPaid -= deleted.AmountPaid;
                Updated?.Invoke(currentSubscription);
            }
        }

        private void OnPaymentCreated(PaymentCreated created)
        {
            _logger.LogInformation(LogFlag + "update Subscription");

            Subscription? currentSubscription = _subscriptions.FirstOrDefault(y => y.Id == created.SubscriptionId);
            if (currentSubscription != null)
            {
                currentSubscription.TotalPaid += created.AmountPaid;
                Updated?.Invoke(currentSubscription);
            }
        }
        public async Task Add(Subscription entity)
        {
            _logger.LogInformation(LogFlag + "add subscription");
            await _subscriptionDataService.Create(entity);
            _subscriptions.Add(entity);
            _allSubscriptions.Add(entity);
            Created?.Invoke(entity);
        }
        public async Task Delete(int entity_id)
        {
            _logger.LogInformation(LogFlag + "delete subscription");
            bool deleted = await _subscriptionDataService.Delete(entity_id);
            int currentIndex = _subscriptions.FindIndex(y => y.Id == entity_id);
            _subscriptions.RemoveAt(currentIndex);
            _allSubscriptions.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAllByPlayer(int playerId)
        {
            _logger.LogInformation(LogFlag + "get all subscription");
            IEnumerable<Subscription> subscriptions = await _getPlayerTransactionService.GetAllByPlayer(playerId);
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
            int days = SettingsManager.Current.SubscriptionRemainderExpirationDays;
            _logger.LogInformation(LogFlag + "get all active subscription");
            IEnumerable<Subscription> subscriptions = await _subscriptionRenewService.GetAll(days);
            _allSubscriptions.Clear();
            _allSubscriptions.AddRange(subscriptions);
            AllLoaded?.Invoke();
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
           Subscription subscription = await _subscriptionDataService.Update(entity);
            RefreshUi(subscription);
        }
        private void RefreshUi(Subscription entity)
        {
            int currentIndex = _subscriptions.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _subscriptions[currentIndex] = entity;
                _allSubscriptions[currentIndex] = entity;
            }
            else
            {
                _subscriptions.Add(entity);
                _allSubscriptions.Add(entity);
            }
            Updated?.Invoke(entity);
        }
        public void RemoveSubscriptionPayments(int entityId, int playerPaymentId)
        {
            _logger.LogInformation(LogFlag + "update Subscription");

            Subscription? currentSubscription = _subscriptions.FirstOrDefault(y => y.Id == entityId);

            if (currentSubscription != null)
            {
                PlayerPayment? existPayment = currentSubscription.Payments?.FirstOrDefault(x => x.Id == playerPaymentId);
                if (existPayment != null)
                {
                    currentSubscription.Payments?.Remove(existPayment);
                }

                Updated?.Invoke(currentSubscription);

            }

        }
        public async Task Stop(Subscription entity, DateTime stopDate)
        {
            _logger.LogInformation(LogFlag + "stop subscription");
            entity.EndDate = stopDate;
            entity.IsStopped = true;
            await _subscriptionDataService.Update(entity);


            int currentIndex = _subscriptions.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _subscriptions[currentIndex] = entity;
                _allSubscriptions[currentIndex] = entity;
            }
            else
            {
                _subscriptions.Add(entity);
                _allSubscriptions.Add(entity);
            }
            Updated?.Invoke(entity);
        }
        public async Task MarkAsRenewed(int entityId)
        {
            await _subscriptionRenewService.MarkAsRenewed(entityId);

            int currentIndex = _subscriptions.FindIndex(y => y.Id == entityId);

            if (currentIndex != -1)
            {
                _subscriptions[currentIndex].IsRenewed = true;
                _allSubscriptions[currentIndex].IsRenewed = true;
                Updated?.Invoke(_subscriptions[currentIndex]);
            }
           
        }

    }
}
