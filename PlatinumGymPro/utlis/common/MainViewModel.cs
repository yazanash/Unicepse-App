//using PlatinumGymPro.Models;
using PlatinumGymPro.State.Navigator;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
       
        public INavigator Navigator { get; set; }
        //private readonly NavigationStore _navigationStore;
        public NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playerStore;
        private readonly SportDataStore _sportStore;
        private readonly EmployeeStore _employeeStore;
        private readonly ExpensesDataStore _expensesStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly MetricDataStore _metricDataStore;
        private readonly RoutineDataStore _routineDataStore;
        public MainViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SportDataStore sportStore, EmployeeStore employeeStore, ExpensesDataStore expensesStore, SubscriptionDataStore subscriptionDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore)
        {
            _navigatorStore = navigatorStore;
            _playerStore = playerStore;
            _sportStore = sportStore;
            _employeeStore = employeeStore;
            _expensesStore = expensesStore;
            _paymentDataStore = paymentDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;

            Navigator = new Navigator(_navigatorStore, _playerStore, _sportStore, _employeeStore, _expensesStore, _subscriptionDataStore, _paymentDataStore, _metricDataStore,_routineDataStore);
            Navigator.CurrentViewModel = new HomeViewModel();



            //_navigationStore.CurrentViewModelChanged += NavigationStore_CurrentViewModelChanged;
        }

        //private void NavigationStore_CurrentViewModelChanged()
        //{
        //    OnPropertyChanged(nameof(CurrentViewModel));
        //}

        //public ViewModelBase? CurrentViewModel =>_navigationStore.CurrentViewModel;

    }
}
