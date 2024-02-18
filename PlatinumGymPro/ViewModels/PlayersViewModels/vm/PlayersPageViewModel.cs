﻿using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.State.Navigator;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels
{
    public class PlayersPageViewModel:ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playerStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly SportDataStore _sportDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly MetricDataStore _metricDataStore;
        private readonly RoutineDataStore _routineDataStore;
        //private SportStore _sportStore;
        //private  TrainerStore _trainerStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public PlayersPageViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore)
        {
            _navigatorStore = navigatorStore;
            _playerStore = playerStore;
            _subscriptionDataStore = subscriptionDataStore;
            _sportDataStore = sportDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            navigatorStore.CurrentViewModel = CreatePlayersViewModel(_navigatorStore, _playerStore, _subscriptionDataStore, _sportDataStore, _paymentDataStore, _metricDataStore, _routineDataStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private PlayerListViewModel CreatePlayersViewModel(NavigationStore navigatorStore,PlayersDataStore playerStore,SubscriptionDataStore subscriptionDataStore,SportDataStore sportDataStore,PaymentDataStore paymentDataStore, MetricDataStore _metricDataStore,RoutineDataStore routineDataStore)
        {
            return PlayerListViewModel.LoadViewModel( navigatorStore, playerStore, subscriptionDataStore, sportDataStore, paymentDataStore,_metricDataStore, routineDataStore);
        }

    }
}
