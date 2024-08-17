﻿using Unicepse.ViewModels;
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
                        _navigator.CurrentViewModel = new HomeNavViewModel(navigator,_playersStore, _playersAttendenceStore1, _employeeStore,_subscriptionDataStore);
                        break;
                    case ViewType.Players:
                        _navigator.CurrentViewModel = new PlayersPageViewModel(navigator, _playersStore, _subscriptionDataStore, _sportStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore1);
                        break;
                    case ViewType.Sport:
                        _navigator.CurrentViewModel = new SportsViewModel(navigator, _sportStore, _employeeStore,_subscriptionDataStore);
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
        private HomeViewModel CreateHomeViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore,NavigationStore navigationStore,SubscriptionDataStore _subscriptionDataStore)
        {
            return HomeViewModel.LoadViewModel(playersDataStore, playersAttendenceStore, employeeStore, navigationStore, _subscriptionDataStore);
        }

    }
}