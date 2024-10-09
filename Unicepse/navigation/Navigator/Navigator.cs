using Unicepse.Core.Models;
using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.navigation.Navigator
{
    public class Navigator : ObservableObject, INavigator
    {
        private readonly NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playersStore;
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
        private readonly GymStore _gymStore;
        private readonly LicenseDataStore _licenseDataStore;
        private readonly AuthenticationStore? _authenticationStore;
        private readonly MainWindowViewModel _mainWindowViewModel;
        public Navigator(NavigationStore navigatorStore, PlayersDataStore playersStore,
            SportDataStore sportStore, EmployeeStore employeeStore,
            ExpensesDataStore expensesStore, SubscriptionDataStore subscriptionDataStore,
            PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore, UsersDataStore usersDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, GymStore gymStore, LicenseDataStore licenseDataStore, AuthenticationStore? authenticationStore, MainWindowViewModel mainWindowViewModel)
        {
            _navigatorStore = navigatorStore;
            _playersStore = playersStore;
            _sportStore = sportStore;
            _employeeStore = employeeStore;
            _expensesStore = expensesStore;
            _subscriptionDataStore = subscriptionDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _usersDataStore = usersDataStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            _gymStore = gymStore;
            _licenseDataStore = licenseDataStore;
            _authenticationStore = authenticationStore;
            _mainWindowViewModel = mainWindowViewModel;
            //LogoutCommand = new NavaigateCommand<AuthViewModel>(new NavigationService<AuthViewModel>(_navigatorStore, () => new AuthViewModel(_navigatorStore)));

        }

        private ViewModelBase? _CurrentViewModel;


        public ViewModelBase CurrentViewModel
        {
            get { return _CurrentViewModel!; }
            set { _CurrentViewModel = value; OnPropertChanged(nameof(CurrentViewModel)); }

        }
        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value; OnPropertChanged(nameof(IsOpen)); }

        }
        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this, _playersStore, _sportStore, _employeeStore, _expensesStore, _subscriptionDataStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore, _usersDataStore, _dausesDataStore, _creditsDataStore, _gymStore, _licenseDataStore, _authenticationStore, _mainWindowViewModel);
        //public ICommand LogoutCommand { get; }

    }
}
