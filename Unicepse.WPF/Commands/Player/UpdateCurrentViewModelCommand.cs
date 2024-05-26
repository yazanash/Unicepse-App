using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels;
using Unicepse.WPF.ViewModels.Accountant;
using Unicepse.WPF.ViewModels.Authentication;
using Unicepse.WPF.ViewModels.Expenses;
using Unicepse.WPF.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.WPF.navigation.Stores;
using Unicepse.WPF.utlis.common;
using Unicepse.WPF.navigation.Navigator;
using Unicepse.WPF.ViewModels.PlayersViewModels.vm;
using Unicepse.WPF.ViewModels.SportsViewModels;
using Unicepse.WPF.ViewModels.Employee.TrainersViewModels;

namespace Unicepse.WPF.Commands.Player
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

        public UpdateCurrentViewModelCommand(INavigator navigator, PlayersDataStore playersStore,
            SportDataStore sportStore, EmployeeStore employeeStore, ExpensesDataStore expensesStore,
            SubscriptionDataStore subscriptionDataStore, PaymentDataStore paymentDataStore,
            MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore1, UsersDataStore usersDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, GymStore gymStore)
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
                        _navigator.CurrentViewModel = CreateHomeViewModel(_playersStore, _playersAttendenceStore1, _employeeStore);
                        break;
                    case ViewType.Players:
                        _navigator.CurrentViewModel = new PlayersPageViewModel(navigator, _playersStore, _subscriptionDataStore, _sportStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore1);
                        break;
                    case ViewType.Sport:
                        _navigator.CurrentViewModel = new SportsViewModel(navigator, _sportStore, _employeeStore);
                        break;
                    case ViewType.Trainer:
                        _navigator.CurrentViewModel = new TrainersViewModel(navigator, _employeeStore, _sportStore, _subscriptionDataStore, _dausesDataStore, _creditsDataStore);
                        break;
                    case ViewType.Users:
                        _navigator.CurrentViewModel = new UsersViewModel(navigator, _usersDataStore, _employeeStore);
                        break;
                    case ViewType.Accounting:
                        _navigator.CurrentViewModel = new AccountingViewModel(navigator, _expensesStore, _paymentDataStore, _gymStore);
                        break;
                    case ViewType.About:
                        _navigator.CurrentViewModel = new AppInfoViewModel();
                        break;
                    default:
                        break;
                }

            }
        }
        private HomeViewModel CreateHomeViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore)
        {
            return HomeViewModel.LoadViewModel(playersDataStore, playersAttendenceStore, employeeStore);
        }

    }
}