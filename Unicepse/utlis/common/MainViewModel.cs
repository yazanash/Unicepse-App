using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.navigation.Navigator;
using Unicepse.Stores;
using Unicepse.navigation.Stores;
using System.Windows.Media;

namespace Unicepse.utlis.common
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
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly UsersDataStore _usersDataStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly GymStore _gymStore;
        private readonly BackgroundServiceStore _backgroundServiceStore;
        private readonly AuthenticationStore _authenticationStore;
        public MainViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SportDataStore sportStore, EmployeeStore employeeStore, ExpensesDataStore expensesStore, SubscriptionDataStore subscriptionDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore, UsersDataStore usersDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, GymStore gymStore, BackgroundServiceStore backgroundServiceStore, AuthenticationStore authenticationStore)
        {
            _navigatorStore = navigatorStore;
            _gymStore = gymStore;
            _playerStore = playerStore;
            _sportStore = sportStore;
            _employeeStore = employeeStore;
            _expensesStore = expensesStore;
            _paymentDataStore = paymentDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _usersDataStore = usersDataStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            _authenticationStore = authenticationStore;

            _backgroundServiceStore = backgroundServiceStore;
            _backgroundServiceStore.StateChanged += _backgroundServiceStore_StateChanged;
            Navigator = new Navigator(_navigatorStore, _playerStore, _sportStore, _employeeStore, _expensesStore, _subscriptionDataStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore, _usersDataStore, _dausesDataStore, _creditsDataStore, _gymStore);

            Navigator.CurrentViewModel = new HomeNavViewModel(new NavigationStore(), _playerStore, _playersAttendenceStore, _employeeStore,_subscriptionDataStore);

            StatusBarViewModel = new StatusBarViewModel(_authenticationStore.CurrentAccount.UserName);

            //_navigationStore.CurrentViewModelChanged += NavigationStore_CurrentViewModelChanged;
        }
        private HomeViewModel CreateHomeViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore,NavigationStore navigationStore,SubscriptionDataStore subscriptionDataStore)
        {
            return HomeViewModel.LoadViewModel(playersDataStore, playersAttendenceStore, employeeStore, navigationStore,subscriptionDataStore);
        }
        private void _backgroundServiceStore_StateChanged(string? obj,bool connectionStatus)
        {
            StatusBarViewModel.BackMessage = obj;
            StatusBarViewModel.Connection=connectionStatus ? Brushes.Green : Brushes.Red;
        }
        public StatusBarViewModel StatusBarViewModel { get; set; }
        //private void NavigationStore_CurrentViewModelChanged()
        //{
        //    OnPropertyChanged(nameof(CurrentViewModel));
        //}

        //public ViewModelBase? CurrentViewModel =>_navigationStore.CurrentViewModel;

    }
}
