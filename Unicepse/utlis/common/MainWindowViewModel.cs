using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.navigation.Stores;

namespace Unicepse.utlis.common
{
    public class MainWindowViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly GymStore _gymStore;
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
        private readonly DausesDataStore _dausesDataStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly BackgroundServiceStore _backgroundServiceStore;
        private readonly AuthenticationStore _authenticationStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;

        public MainWindowViewModel(NavigationStore navigatorStore,
            PlayersDataStore playerStore, SportDataStore sportStore,
            EmployeeStore employeeStore, ExpensesDataStore expensesStore,
            SubscriptionDataStore subscriptionDataStore, PaymentDataStore paymentDataStore,
            MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore,
            UsersDataStore usersDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, GymStore gymStore, BackgroundServiceStore backgroundServiceStore, AuthenticationStore authenticationStore)
        {

            _navigatorStore = navigatorStore;
            _playerStore = playerStore;
            _sportStore = sportStore;
            _employeeStore = employeeStore;
            _expensesStore = expensesStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _routineDataStore = routineDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            _usersDataStore = usersDataStore;
            _gymStore = gymStore;
            _backgroundServiceStore = backgroundServiceStore;
            _authenticationStore = authenticationStore;

            _navigatorStore.CurrentViewModel = new MainViewModel(_navigatorStore, _playerStore, _sportStore,
                _employeeStore, _expensesStore, _subscriptionDataStore,
                _paymentDataStore, _metricDataStore, _routineDataStore,
                _playersAttendenceStore, _usersDataStore,
                _dausesDataStore, _creditsDataStore, _gymStore, _backgroundServiceStore, _authenticationStore);
            _navigatorStore.CurrentViewModelChanged += _navigatorStore_CurrentViewModelChanged; ;
        }


        private void _navigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
