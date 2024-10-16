using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Employee.TrainersViewModels
{
    public class TrainersViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly EmployeeStore _employeeStore;
        private readonly SportDataStore _sportDataStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly LicenseDataStore _licenseDataStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public TrainersViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, SportDataStore sportDataStore, SubscriptionDataStore subscriptionDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, LicenseDataStore licenseDataStore)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            _sportDataStore = sportDataStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            _licenseDataStore = licenseDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            navigatorStore.CurrentViewModel = CreateTrainerViewModel(_navigatorStore, _employeeStore, _sportDataStore, _subscriptionDataStore, _dausesDataStore, _creditsDataStore,_licenseDataStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
          
        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private TrainersListViewModel CreateTrainerViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, SportDataStore sportDataStore, SubscriptionDataStore subscriptionDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore,LicenseDataStore licenseDataStore)
        {
            return TrainersListViewModel.LoadViewModel(navigatorStore, employeeStore, sportDataStore, subscriptionDataStore, dausesDataStore, creditsDataStore,licenseDataStore);
        }
    }
}
