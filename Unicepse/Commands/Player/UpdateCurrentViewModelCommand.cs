using Unicepse.ViewModels;
using Unicepse.ViewModels.Accountant;
using Unicepse.ViewModels.Authentication;
using Unicepse.ViewModels.Expenses;
using Unicepse.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.ViewModels.PlayersViewModels.vm;
using Unicepse.ViewModels.SportsViewModels;
using Unicepse.ViewModels.Employee.TrainersViewModels;
using Unicepse.Stores;
using Unicepse.navigation.Navigator;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;
using Unicepse.Core.Common;
using System.Windows;

namespace Unicepse.Commands.Player
{
    public class UpdateCurrentViewModelCommand : CommandBase
    {
        public INavigator _navigator;
        private readonly PlayersDataStore _playersStore;
        private readonly SportDataStore _sportStore;
        private readonly EmployeeStore _employeeStore;
        private readonly ExpensesDataStore _expensesStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly MetricDataStore _metricDataStore;
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore1;
        private readonly UsersDataStore _usersDataStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly GymStore _gymStore;
        private readonly LicenseDataStore _licenseDataStore;
        private readonly AuthenticationStore? _authenticationStore;
        private readonly MainWindowViewModel _mainWindowViewModel;
        public UpdateCurrentViewModelCommand(INavigator navigator, PlayersDataStore playersStore,
            SportDataStore sportStore, EmployeeStore employeeStore, ExpensesDataStore expensesStore,
            SubscriptionDataStore subscriptionDataStore, PaymentDataStore paymentDataStore,
            MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore1, UsersDataStore usersDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, GymStore gymStore, LicenseDataStore licenseDataStore, AuthenticationStore? authenticationStore, MainWindowViewModel mainWindowViewModel)
        {
            _navigator = navigator;
            _playersStore = playersStore;
            _sportStore = sportStore;
            _employeeStore = employeeStore;
            _expensesStore = expensesStore;
            _subscriptionDataStore = subscriptionDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _playersAttendenceStore1 = playersAttendenceStore1;
            _usersDataStore = usersDataStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            _gymStore = gymStore;
            _licenseDataStore = licenseDataStore;
            _authenticationStore = authenticationStore;
            _mainWindowViewModel = mainWindowViewModel;
        }
        public override bool CanExecute(object? parameter)
        {
            bool isAble = false;
            if (parameter is ViewType)
            {
               
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Home:
                        if (_authenticationStore!.CurrentAccount!.Role != Roles.Accountant)
                            isAble = true;
                        break;
                    case ViewType.Players:
                        if (_authenticationStore!.CurrentAccount!.Role != Roles.Accountant)
                            isAble = true;
                        break;
                    case ViewType.Sport:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Supervisor)
                            isAble = true;
                        break;
                    case ViewType.Trainer:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Supervisor || _authenticationStore!.CurrentAccount!.Role == Roles.Accountant)
                            isAble = true;
                        break;
                    case ViewType.Users:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin)
                            isAble = true;
                        break;
                    case ViewType.Accounting:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Accountant)
                            isAble = true;
                        break;
                    case ViewType.About:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin)
                            isAble = true;
                        break;
                    case ViewType.Logout:
                            isAble = true;
                        break;
                    default:
                        break;
                }

            }
            return base.CanExecute(parameter) && isAble;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is ViewType)
            {
                NavigationStore navigator = new NavigationStore();
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Home:
                        if (_authenticationStore!.CurrentAccount!.Role != Roles.Accountant)
                            _navigator.CurrentViewModel = new HomeNavViewModel(navigator,_playersStore, _playersAttendenceStore1, _employeeStore,_subscriptionDataStore);
                        break;
                    case ViewType.Players:
                        if (_authenticationStore!.CurrentAccount!.Role != Roles.Accountant)
                            _navigator.CurrentViewModel = new PlayersPageViewModel(navigator, _playersStore, _subscriptionDataStore, _sportStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore1);
                        break;
                    case ViewType.Sport:
                        if(_authenticationStore!.CurrentAccount!.Role==Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Supervisor)
                        _navigator.CurrentViewModel = new SportsViewModel(navigator, _sportStore, _employeeStore,_subscriptionDataStore);
                        break;
                    case ViewType.Trainer:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Supervisor || _authenticationStore!.CurrentAccount!.Role == Roles.Accountant)
                            _navigator.CurrentViewModel = new TrainersViewModel(navigator, _employeeStore, _sportStore, _subscriptionDataStore, _dausesDataStore, _creditsDataStore);
                        break;
                    case ViewType.Users:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin)
                            _navigator.CurrentViewModel = new UsersViewModel(navigator, _usersDataStore, _employeeStore,_authenticationStore);
                        break;
                    case ViewType.Accounting:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Accountant)
                            _navigator.CurrentViewModel = new AccountingViewModel(navigator, _expensesStore, _paymentDataStore, _gymStore);
                        break;
                    case ViewType.About:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin)
                            _navigator.CurrentViewModel = CreateAppInfo(_licenseDataStore);
                        break;
                    case ViewType.Logout:
                        if (MessageBox.Show("سيتم تسجيل خروجك , هل انت متاكد", "تنبيه", MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            _authenticationStore!.Logout();
                            _mainWindowViewModel.OnLogoutAction();
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        private AppInfoViewModel CreateAppInfo(LicenseDataStore licenseDataStore)
        {
            return AppInfoViewModel.LoadViewModel(licenseDataStore);
        }

    }
}