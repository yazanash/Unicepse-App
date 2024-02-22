﻿using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Payment;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.Sport;
using PlatinumGym.Core.Models.Subscription;
using PlatinumGym.Entityframework.Services;
using PlatinumGymPro.ViewModels.SportsViewModels;
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
        public event Action? Loaded;
        public event Action<Subscription>? Updated;
        public event Action<int>? Deleted;


        private readonly SubscriptionDataService _subscriptionDataService;
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
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll(Player player)
        {
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetAll(player);
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            Loaded?.Invoke();
        }
        public async Task GetAll()
        {
            IEnumerable<Subscription> subscriptions = await _subscriptionDataService.GetAll();
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            Loaded?.Invoke();
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
