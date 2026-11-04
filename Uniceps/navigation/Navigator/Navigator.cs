using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Core.Common;
using Uniceps.Core.Models;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.Accountant;
using Uniceps.ViewModels.Authentication;
using Uniceps.ViewModels.DashboardViewModels;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.navigation.Navigator
{
    public class Navigator : ObservableObject, INavigator
    {
        private readonly INavigationService _navigationService;
        private readonly AuthenticationStore _authenticationStore;
        private readonly AccountStore _accountStore;

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set { _isOpen = value; OnPropertChanged(nameof(IsOpen)); }
        }

        public ICommand NavigateCommand { get; }

        public Navigator(
            INavigationService navigationService,
            AuthenticationStore authenticationStore,
            AccountStore accountStore)
        {
            _navigationService = navigationService;
            _authenticationStore = authenticationStore;
            _accountStore = accountStore;

            NavigateCommand = new RelayCommand<object?>(ExecuteNavigate);
            NavigateCommand.Execute(ViewType.Home);
        }

        private void ExecuteNavigate(object? parameter)
        {
            if (parameter is not ViewType viewType) return;

            var role = _authenticationStore.CurrentAccount?.Role;

            // التحقق من الصلاحيات والتنقل عبر الـ INavigationService مباشرة
            switch (viewType)
            {
                case ViewType.Dashboard:
                    if (role == Roles.Admin)
                        _navigationService.Navigate<DashboardViewModel>();
                    break;

                case ViewType.Home:
                    if (role != Roles.Accountant)
                        _navigationService.Navigate<SubscriptionMainViewModel>();
                    else
                        _navigationService.Navigate<AccountingViewModel>();
                    break;

                case ViewType.PlayersLog:
                    if (_accountStore != null && role != Roles.Accountant)
                        _navigationService.Navigate<HomeViewModel>(); // أو الصفحة الخاصة بدخول وخروج اللاعبين
                    else
                        _navigationService.Navigate<PremiumViewModel>();
                    break;

                case ViewType.Players:
                    if (role == Roles.Admin || role == Roles.Supervisor)
                        _navigationService.Navigate<PlayerListViewModel>();
                    break;

                case ViewType.Sport:
                    if (role == Roles.Admin || role == Roles.Supervisor)
                        _navigationService.Navigate<SportListViewModel>();
                    break;

                case ViewType.Trainer:
                    if (role == Roles.Admin || role == Roles.Supervisor || role == Roles.Accountant)
                        _navigationService.Navigate<TrainersListViewModel>();
                    break;

                case ViewType.Exercises:
                    if (_accountStore != null && (role == Roles.Admin || role == Roles.Supervisor))
                        _navigationService.Navigate<RoutineListViewModel>();
                    else
                        _navigationService.Navigate<PremiumViewModel>();
                    break;

                case ViewType.Users:
                    if (_accountStore != null && role == Roles.Admin)
                        _navigationService.Navigate<UsersListViewModel>();
                    else
                        _navigationService.Navigate<PremiumViewModel>();
                    break;

                case ViewType.Accounting:
                    if (role == Roles.Admin || role == Roles.Accountant)
                        _navigationService.Navigate<AccountingViewModel>();
                    break;

                case ViewType.About:
                    if (role == Roles.Admin)
                        _navigationService.Navigate<AppInfoViewModel>();
                    break;
            }
        }
    }
}
