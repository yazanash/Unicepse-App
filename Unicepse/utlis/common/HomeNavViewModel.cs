using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.navigation.Stores;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersViewModels;

namespace Unicepse.utlis.common
{
    public class HomeNavViewModel:ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly EmployeeStore _employeeStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;

        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public HomeNavViewModel(NavigationStore navigatorStore, PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore, SubscriptionDataStore subscriptionDataStore)
        {
            _navigatorStore = new NavigationStore();
            _playersDataStore = playersDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _employeeStore = employeeStore;
            _subscriptionDataStore = subscriptionDataStore;
          ;

            _navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            _navigatorStore.CurrentViewModel = CreateHomeViewModel(playersDataStore, _playersAttendenceStore, _employeeStore, _navigatorStore, _subscriptionDataStore);
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private HomeViewModel CreateHomeViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore)
        {
            return HomeViewModel.LoadViewModel(playersDataStore, playersAttendenceStore, employeeStore, navigationStore, subscriptionDataStore);
        }
    }
}
