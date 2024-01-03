using PlatinumGymPro.Commands;
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
        //private SportStore _sportStore;
        //private  TrainerStore _trainerStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public PlayersPageViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionDataStore)
        {
            _navigatorStore = navigatorStore;
            _playerStore = playerStore;
            _subscriptionDataStore = subscriptionDataStore;

            //_sportStore = sportStore;
            //_trainerStore = trainerStore;
            navigatorStore.CurrentViewModel = CreatePlayersViewModel(_navigatorStore, _playerStore, _subscriptionDataStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private PlayerListViewModel CreatePlayersViewModel(NavigationStore navigatorStore,PlayersDataStore playerStore,SubscriptionDataStore subscriptionDataStore)
        {
            return PlayerListViewModel.LoadViewModel( navigatorStore, playerStore, subscriptionDataStore);
        }

    }
}
