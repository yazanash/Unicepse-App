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
        public NavigationStore _navigationStore;
        private readonly HomeNavViewModel _homeNavViewModel;
        private readonly PlayerListViewModel _playersPageViewModel;
        private readonly SportListViewModel _sportsViewModel;
        private readonly TrainersListViewModel _trainersViewModel;
        private readonly UsersListViewModel _usersViewModel;
        private readonly AccountingViewModel _accountingViewModel;

        private readonly LicenseDataStore _licenseDataStore;
        private readonly AuthenticationStore? _authenticationStore;
        public UpdateCurrentViewModelCommand(NavigationStore navigationStore, AuthenticationStore? authenticationStore,
            HomeNavViewModel homeNavViewModel, PlayerListViewModel playersPageViewModel,
            SportListViewModel sportsViewModel, TrainersListViewModel trainersViewModel,
            UsersListViewModel usersViewModel, AccountingViewModel accountingViewModel,
            LicenseDataStore licenseDataStore)
        {
            _navigationStore = navigationStore;
            _authenticationStore = authenticationStore;
            _homeNavViewModel = homeNavViewModel;
            _playersPageViewModel = playersPageViewModel;
            _sportsViewModel = sportsViewModel;
            _trainersViewModel = trainersViewModel;
            _usersViewModel = usersViewModel;
            _accountingViewModel = accountingViewModel;
            _licenseDataStore = licenseDataStore;
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
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Home:
                        if (_authenticationStore!.CurrentAccount!.Role != Roles.Accountant)
                            _navigationStore.CurrentViewModel = _homeNavViewModel;
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
                    case ViewType.Users:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin)
                            _navigationStore.CurrentViewModel = _usersViewModel;
                        break;
                    case ViewType.Accounting:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin || _authenticationStore!.CurrentAccount!.Role == Roles.Accountant)
                            _navigationStore.CurrentViewModel = _accountingViewModel;
                        break;
                    case ViewType.About:
                        if (_authenticationStore!.CurrentAccount!.Role == Roles.Admin)
                            _navigationStore.CurrentViewModel = CreateAppInfo(_licenseDataStore);
                        break;
                    case ViewType.Logout:
                        if (MessageBox.Show("سيتم تسجيل خروجك , هل انت متاكد", "تنبيه", MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            _authenticationStore!.Logout();
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