using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.SportsViewModels
{
    public class SportsViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly SportDataStore _sportStore;
        private readonly EmployeeStore _trainerStore;
        private SubscriptionDataStore _subscriptionDataStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public SportsViewModel(NavigationStore navigatorStore, SportDataStore sportStore, EmployeeStore trainerStore, SubscriptionDataStore subscriptionDataStore)
        {
            _navigatorStore = new NavigationStore();
            _sportStore = sportStore;
            _subscriptionDataStore = subscriptionDataStore;

            _trainerStore = trainerStore;
            _navigatorStore.CurrentViewModel = CreateSportViewModel(_navigatorStore, _sportStore, _trainerStore, _subscriptionDataStore);
            _navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private SportListViewModel CreateSportViewModel(NavigationStore navigatorStore, SportDataStore sportStore, EmployeeStore employeeStore, SubscriptionDataStore subscriptionDataStore)
        {
            return SportListViewModel.LoadViewModel(navigatorStore, sportStore, employeeStore,subscriptionDataStore);
        }
       
    }
}
