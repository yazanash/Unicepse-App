using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Commands.SystemAuthCommands;
using Uniceps.Core.Common;
using Uniceps.Helpers;
using Uniceps.navigation;
using Uniceps.navigation.Navigator;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.utlis.ComponentsViewModels;
using Uniceps.ViewModels.Accountant;
using Uniceps.ViewModels.Authentication;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.ViewModels.SystemAuthViewModels;
using Uniceps.Views;
using Uniceps.Views.SystemAuthViews;

namespace Uniceps.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public INavigator Navigator { get; set; }
        private readonly UsersDataStore _usersDataStore;
        private readonly AccountingViewModel _accountingViewModel;
        private readonly HomeViewModel _homeNavViewModel;
        private readonly BackgroundServiceStore _backgroundServiceStore;
        private readonly AuthenticationStore _authenticationStore;
        private readonly AccountStore _accountStore;
        private readonly SystemProfileCreationViewModel _systemProfileCreationViewModel;
        private readonly SystemSubscriptionStore _systemSubscriptionStore;
        private readonly SystemLoginViewModel _systemLoginViewModel;
        private readonly SubscriptionMainViewModel _subscriptionMainViewModel;
        private readonly UserFlowService _userFlowService;
        private readonly AppInfoViewModel _appInfoViewModel;
        public StatusBarViewModel? StatusBarViewModel { get; set; }
        public NotificationBarViewModel NotificationBarViewModel { get; set; }
        private readonly NavigationStore _navigationStore;
        public string ThemeIcon => CurrentTheme != AppTheme.Light ? "WeatherSunny" : "WeatherNight";
        private AppTheme _currentTheme;
        public AppTheme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                _currentTheme = value;
                OnPropertyChanged(nameof(CurrentTheme));
                OnPropertyChanged(nameof(ThemeIcon));
            }
        }

        public MainWindowViewModel(UsersDataStore usersDataStore, BackgroundServiceStore backgroundServiceStore, AuthenticationStore authenticationStore, INavigator navigator, AccountingViewModel accountingViewModel, HomeViewModel homeNavViewModel, AccountStore accountStore, SystemProfileCreationViewModel systemProfileCreationViewModel, SystemSubscriptionStore systemSubscriptionStore, NavigationStore navigationStore, SystemLoginViewModel systemLoginViewModel, SubscriptionMainViewModel subscriptionMainViewModel, UserFlowService userFlowService, AppInfoViewModel appInfoViewModel)
        {
            Navigator = navigator;
            _navigationStore = navigationStore;
            _systemLoginViewModel = systemLoginViewModel;
            _systemLoginViewModel.OTPVerifiedAction += _systemLoginViewModel_OTPVerifiedAction;
            _accountingViewModel = accountingViewModel;
            _homeNavViewModel = homeNavViewModel;
            _usersDataStore = usersDataStore;
            _authenticationStore = authenticationStore;
            _backgroundServiceStore = backgroundServiceStore;
            _accountStore = accountStore;
            _systemProfileCreationViewModel = systemProfileCreationViewModel;
            _userFlowService = userFlowService;
            _appInfoViewModel = appInfoViewModel;
            _systemProfileCreationViewModel.ProfileCreatedAction += _systemProfileCreationViewModel_ProfileCreatedAction;

            _systemSubscriptionStore = systemSubscriptionStore;
            _systemSubscriptionStore.Created += _systemSubscriptionStore_Created;

            _subscriptionMainViewModel = subscriptionMainViewModel;

            NotificationBarViewModel = new NotificationBarViewModel();
            _backgroundServiceStore.StateChanged += _backgroundServiceStore_StateChanged;
            _backgroundServiceStore.SyncStatus += _backgroundServiceStore_SyncStatus;
            _usersDataStore.Updated += _usersDataStore_Updated;
            _accountStore.UserContextChanged += _accountStore_UserContextChanged;
            _appInfoViewModel.ProfileUpdated += _appInfoViewModel_ProfileUpdated;
            ValidateUserContext();
            var savedTheme = Properties.Settings.Default.AppTheme;
            if (Enum.TryParse(savedTheme, out AppTheme theme))
            {
                CurrentTheme = theme;
            }

          
        }

        private void _appInfoViewModel_ProfileUpdated()
        {
            RefreshUserContext.Execute(null);
        }

        private void _systemSubscriptionStore_Created()
        {
            RefreshUserContext.Execute(null);
        }

        private void _systemLoginViewModel_OTPVerifiedAction()
        {
            RefreshUserContext.Execute(null);
        }

        private void _systemProfileCreationViewModel_ProfileCreatedAction()
        {
            RefreshUserContext.Execute(null);
        }
        public ICommand RefreshUserContext => new AsyncRelayCommand(Refresh);

        public async Task Refresh()
        {
            await _userFlowService.RefreshUserContextAsync() ;
        }

        public ICommand ChangeThemeCommand => new RelayCommand(ChangeTheme);

        private void ChangeTheme()
        {
            if (CurrentTheme == AppTheme.Light)
                CurrentTheme = AppTheme.Dark;
            else
                CurrentTheme = AppTheme.Light;
            ThemeService.ApplyTheme(CurrentTheme);
        }
        private void _accountStore_UserContextChanged()
        {
            ValidateUserContext();
        }
        void ValidateUserContext()
        {
            switch (_accountStore.UserContext)
            {
                case UserContextState.UnAuthenticated:

                    NotificationBarViewModel.Notification = "قم بتسجيل الدخول وانضم الى مجتمعنا ";
                    NotificationBarViewModel.NotificationBarColor = Brushes.Yellow;
                    NotificationBarViewModel.ActionTitle = "سجل الان";
                    NotificationBarViewModel.HasNotification = true;
                    NotificationBarViewModel.NotificationCommand = new OpenAuthCommand(_systemLoginViewModel);
                    break;
                case UserContextState.NoProfile:
                    NotificationBarViewModel.Notification = "قم باضافة معلومات علاماتك التجارية ودع الامر لنا ";
                    NotificationBarViewModel.NotificationBarColor = Brushes.Blue;
                    NotificationBarViewModel.ActionTitle = "اضافة الملف التعريفي";
                    NotificationBarViewModel.HasNotification = true;
                    NotificationBarViewModel.NotificationCommand = OpenProfileCommand;
                    break;
                case UserContextState.NoSubscription:
                    NavigationService<SubscriptionMainViewModel> _navigationService = new NavigationService<SubscriptionMainViewModel>(_navigationStore, () => _subscriptionMainViewModel);
                    NotificationBarViewModel.Notification = "اشترك باحدى باقاتنا المميزة واحصل على المزيد من الميزات";
                    NotificationBarViewModel.NotificationBarColor = Brushes.Green;
                    NotificationBarViewModel.ActionTitle = "اشترك الان";
                    NotificationBarViewModel.HasNotification = true;
                    NotificationBarViewModel.NotificationCommand = new NavaigateCommand<SystemSubscriptionCreationViewModel>(new NavigationService<SystemSubscriptionCreationViewModel>(_navigationStore, () => SystemSubscriptionCreationViewModel.LoadViewModel(_systemSubscriptionStore, _navigationService)));
                    break;
                case UserContextState.Ready:

                    break;
            }
            PrepareMainViewModel();
        }
        void PrepareMainViewModel()
        {
            if (_accountStore.CurrentAccount == null)
            {
                _accountStore.CurrentAccount = new Core.Models.Authentication.User()
                {
                    OwnerName = "مستخدم غير مسجل",
                    Role = Roles.Admin,
                };
            }


            if (_accountStore.CurrentAccount!.Role == Roles.Accountant)
            {
                Navigator.CurrentViewModel = _accountingViewModel;
            }
            else
            {
                //Navigator.CurrentViewModel = _homeNavViewModel;
                Navigator.CurrentViewModel = _subscriptionMainViewModel;
            }

            StatusBarViewModel = new StatusBarViewModel(_accountStore.CurrentAccount!.UserName,
                _accountStore.CurrentAccount!.Position,
                _accountStore.CurrentAccount!.OwnerName);
            switch (_accountStore.CurrentAccount!.Role)
            {
                case Roles.Admin:
                    StatusBarViewModel.Role = "مدير النظام";
                    break;
                case Roles.User:
                    StatusBarViewModel.Role = "مستخدم";
                    break;
                case Roles.Accountant:
                    StatusBarViewModel.Role = "محاسب";
                    break;
                case Roles.Supervisor:
                    StatusBarViewModel.Role = "مسؤول";
                    break;
            }
            StatusBarViewModel.SyncState = _backgroundServiceStore.SyncStateProp;
            StatusBarViewModel.SyncMessage = _backgroundServiceStore.SyncMessage;
            StatusBarViewModel.BackMessage = _backgroundServiceStore.BackMessage;
            StatusBarViewModel.Connection = _backgroundServiceStore.Connection ? Brushes.Green : Brushes.Red;

            //if (_accountStore.CurrentLicense != null)
            //{
            //    DaysLeft = (int)_licenseDataStore.CurrentLicense.SubscribeEndDate.Subtract(DateTime.Now).TotalDays;
            //    IsExpired = DaysLeft <= 5;
            //}
        }
        public ICommand OpenProfileCommand => new RelayCommand(ProfileCreation);
        public void ProfileCreation()
        {
            CreateProfileViewWindow createProfileViewWindow = new CreateProfileViewWindow() { DataContext = _systemProfileCreationViewModel };
            createProfileViewWindow.ShowDialog();
        }
       
        private void _usersDataStore_Updated(Core.Models.Authentication.User obj)
        {
            if (StatusBarViewModel != null && _accountStore.CurrentAccount!.Id == obj.Id)
            {
                StatusBarViewModel.UserName = _accountStore.CurrentAccount!.UserName;
                StatusBarViewModel.Position = _accountStore.CurrentAccount!.Position;
                StatusBarViewModel.OwnerName = _accountStore.CurrentAccount!.OwnerName;
                switch (_accountStore.CurrentAccount!.Role)
                {
                    case Roles.Admin:
                        StatusBarViewModel.Role = "مدير النظام";
                        break;
                    case Roles.User:
                        StatusBarViewModel.Role = "مستخدم";
                        break;
                    case Roles.Accountant:
                        StatusBarViewModel.Role = "محاسب";
                        break;
                    case Roles.Supervisor:
                        StatusBarViewModel.Role = "مسؤول";
                        break;
                }
            }
        }
        private void _backgroundServiceStore_SyncStatus(bool obj, string? message)
        {
            if (StatusBarViewModel != null)
            {
                StatusBarViewModel.SyncState = obj;
                StatusBarViewModel.SyncMessage = message;
            }

        }
        private void _backgroundServiceStore_StateChanged(string? obj, bool connectionStatus)
        {
            if (StatusBarViewModel != null)
            {
                StatusBarViewModel.BackMessage = obj;
                StatusBarViewModel.Connection = connectionStatus ? Brushes.Green : Brushes.Red;
            }
        }
    }
}
