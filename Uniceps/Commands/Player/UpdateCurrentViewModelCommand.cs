using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Core.Common;
using Uniceps.navigation.Navigator;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.Accountant;
using Uniceps.ViewModels.Authentication;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.Expenses;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.Commands.Player
{
    public class UpdateCurrentViewModelCommand : CommandBase
    {
        public NavigationStore _navigationStore;
        private readonly HomeViewModel _homeNavViewModel;
        private readonly PlayerListViewModel _playersPageViewModel;
        private readonly SportListViewModel _sportsViewModel;
        private readonly TrainersListViewModel _trainersViewModel;
        private readonly RoutineListViewModel _routineListViewModel;
        private readonly UsersListViewModel _usersViewModel;
        private readonly AccountingViewModel _accountingViewModel;
        private readonly AppInfoViewModel _appInfoViewModel;
        private readonly AuthenticationStore? _authenticationStore;
        private readonly AccountStore _accountStore;
        private readonly SubscriptionMainViewModel _subscriptionMainViewModel;
        private readonly PremiumViewModel _premiumViewModel = new PremiumViewModel();
        public UpdateCurrentViewModelCommand(NavigationStore navigationStore, AuthenticationStore? authenticationStore,
            HomeViewModel homeNavViewModel, PlayerListViewModel playersPageViewModel,
            SportListViewModel sportsViewModel, TrainersListViewModel trainersViewModel,
            UsersListViewModel usersViewModel, AccountingViewModel accountingViewModel,
           RoutineListViewModel routineListViewModel, AppInfoViewModel appInfoViewModel, SubscriptionMainViewModel subscriptionMainViewModel, AccountStore accountStore)
        {
            _navigationStore = navigationStore;
            _authenticationStore = authenticationStore;
            _homeNavViewModel = homeNavViewModel;
            _playersPageViewModel = playersPageViewModel;
            _sportsViewModel = sportsViewModel;
            _trainersViewModel = trainersViewModel;
            _usersViewModel = usersViewModel;
            _accountingViewModel = accountingViewModel;
            _routineListViewModel = routineListViewModel;
            _appInfoViewModel = appInfoViewModel;
            _subscriptionMainViewModel = subscriptionMainViewModel;
            _accountStore = accountStore;
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
                    case ViewType.PlayersLog:
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
                    case ViewType.Exercises:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Supervisor)
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
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Home:
                        if (_authenticationStore!.CurrentAccount!.Role != Roles.Accountant)
                            _navigationStore.CurrentViewModel = _subscriptionMainViewModel;
                        else if (_authenticationStore!.CurrentAccount!.Role == Roles.Accountant)
                        {
                            _accountingViewModel.ResetSelection();
                            _navigationStore.CurrentViewModel = _accountingViewModel;
                        }
                        break;
                    case ViewType.PlayersLog:
                        if (_accountStore.SystemSubscription != null)
                        {
                            if (_authenticationStore!.CurrentAccount!.Role != Roles.Accountant)
                                _navigationStore.CurrentViewModel = _homeNavViewModel;
                        }
                        else
                            _navigationStore.CurrentViewModel = _premiumViewModel;
                        break;
                    case ViewType.Players:
                        if (_authenticationStore!.CurrentAccount!.Role != Roles.Accountant)
                            _navigationStore.CurrentViewModel = _playersPageViewModel;
                        break;
                    case ViewType.Sport:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Supervisor)
                            _navigationStore.CurrentViewModel = _sportsViewModel;
                        break;
                    case ViewType.Trainer:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Supervisor || _authenticationStore!.CurrentAccount!.Role == Roles.Accountant)
                            _navigationStore.CurrentViewModel = _trainersViewModel;
                        break;
                    case ViewType.Exercises:
                        if (_accountStore.SystemSubscription != null)
                        {
                            if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Supervisor )
                                _navigationStore.CurrentViewModel = _routineListViewModel;
                        }
                        else
                            _navigationStore.CurrentViewModel = _premiumViewModel;

                        break;
                    case ViewType.Users:
                        if (_accountStore.SystemSubscription != null)
                        {
                            if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin)
                                _navigationStore.CurrentViewModel = _usersViewModel;
                        }
                        else
                            _navigationStore.CurrentViewModel = _premiumViewModel;

                        break;
                    case ViewType.Accounting:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Accountant)
                        {
                            _accountingViewModel.ResetSelection();
                            _navigationStore.CurrentViewModel = _accountingViewModel;
                        }
                        break;
                    case ViewType.About:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin)
                            _navigationStore.CurrentViewModel = _appInfoViewModel;
                        break;
                    default:
                        break;
                }

            }
        }
    }
}