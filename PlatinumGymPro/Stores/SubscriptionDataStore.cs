using PlatinumGym.Core.Models.Payment;
using PlatinumGym.Core.Models.Subscription;
using PlatinumGym.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class SubscriptionDataStore : IDataStore<Subscription>
    {
        public event Action<Subscription>? Created;
        public event Action<IEnumerable<Subscription>>? Loaded;
        public event Action<Subscription>? Updated;
        public event Action<bool>? Deleted;


        private readonly SubscriptionDataService _subscriptionDataService;
        private readonly List<Subscription> _subscriptions;
        private readonly Lazy<Task> _initializeLazy;

        public IEnumerable<Subscription> Subscriptions => _subscriptions;

        public SubscriptionDataStore(SubscriptionDataService subscriptionDataService)
        {
            _subscriptionDataService = subscriptionDataService;
            _subscriptions = new List<Subscription>();
            _initializeLazy = new Lazy<Task>(Initialize);
        }

        public async Task Add(Subscription entity)
        {
            await _subscriptionDataService.Create(entity);
            _subscriptions.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            bool deleted = await _subscriptionDataService.Delete(entity_id);
            int currentIndex = _subscriptions.FindIndex(y => y.Id == entity_id);
            _subscriptions.RemoveAt(currentIndex);
            Deleted?.Invoke(deleted);
        }

        public async Task GetAll()
        {
            await _initializeLazy.Value;
        }

        public async Task Initialize()
        {
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetAll();
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
        }

        public async Task Update(Subscription entity)
        {
            await _subscriptionDataService.Update(entity);
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
