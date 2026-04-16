using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Core.Common;
using Uniceps.Helpers;
using Uniceps.navigation;
using Uniceps.navigation.Navigator;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.SystemServices;
using Uniceps.utlis.ComponentsViewModels;
using Uniceps.ViewModels.Accountant;
using Uniceps.ViewModels.Authentication;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views;
using Microsoft.Toolkit.Uwp.Notifications;
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
        private readonly SubscriptionMainViewModel _subscriptionMainViewModel;
        private readonly AppInfoViewModel _appInfoViewModel;
        private readonly LicenseStore _licenseStore;
        SystemSubscriptionPaymentOptionDialogViewModel _systemSubscription;
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

        public MainWindowViewModel(UsersDataStore usersDataStore, BackgroundServiceStore backgroundServiceStore, AuthenticationStore authenticationStore, INavigator navigator, AccountingViewModel accountingViewModel, HomeViewModel homeNavViewModel, AccountStore accountStore, NavigationStore navigationStore, SubscriptionMainViewModel subscriptionMainViewModel, AppInfoViewModel appInfoViewModel, LicenseStore licenseStore, SystemSubscriptionPaymentOptionDialogViewModel systemSubscription)
        {
            Navigator = navigator;
            _navigationStore = navigationStore;
            _accountingViewModel = accountingViewModel;
            _homeNavViewModel = homeNavViewModel;
            _usersDataStore = usersDataStore;
            _authenticationStore = authenticationStore;
            _backgroundServiceStore = backgroundServiceStore;
            _accountStore = accountStore;
            _appInfoViewModel = appInfoViewModel;
            _subscriptionMainViewModel = subscriptionMainViewModel;
            _systemSubscription = systemSubscription;

            NotificationBarViewModel = new NotificationBarViewModel();
            _backgroundServiceStore.StateChanged += _backgroundServiceStore_StateChanged;
            _backgroundServiceStore.SyncStatus += _backgroundServiceStore_SyncStatus;
            _usersDataStore.Updated += _usersDataStore_Updated;
            var savedTheme = SettingsManager.Current.AppTheme;
            if (Enum.TryParse(savedTheme, out AppTheme theme))
            {
                CurrentTheme = theme;
            }
            if (IsBackupNeeded())
            {
                NotificationManager.SendBackupNeededNotification();
            }
            PrepareMainViewModel();
            _licenseStore = licenseStore;
            _licenseStore.LicenseChanged += _licenseStore_LicenseChanged;
            if (!_licenseStore.Current.IsFullVersion)
            {
                NotificationBarViewModel.ActionTitle = "اشترك الان";
                NotificationBarViewModel.Notification = "نسخة uniceps التجريبية";
                NotificationBarViewModel.HasNotification = true;
                NotificationBarViewModel.NotificationCommand = new RelayCommand(ExecuteOpenLicensePickerCommand);
            }
            else
            {
                NotificationBarViewModel.HasNotification = false;
            }

        }

        private void _licenseStore_LicenseChanged()
        {
            if (!_licenseStore.Current.IsFullVersion)
            {
                NotificationBarViewModel.ActionTitle = "اشترك الان";
                NotificationBarViewModel.Notification = "نسخة uniceps التجريبية";
                NotificationBarViewModel.HasNotification = true;
                NotificationBarViewModel.NotificationCommand = new RelayCommand(ExecuteOpenLicensePickerCommand);
            }
            else
            {
                NotificationBarViewModel.HasNotification = false;
            }
        }
        private void ExecuteOpenLicensePickerCommand()
        {
            SystemSubscriptionPaymentOptionDialog systemSubscriptionPaymentOptionDialog = new SystemSubscriptionPaymentOptionDialog();
            systemSubscriptionPaymentOptionDialog.DataContext = _systemSubscription;
            systemSubscriptionPaymentOptionDialog.ShowDialog();

        }
        public bool IsBackupNeeded()
        {
            DateTime last = SettingsManager.Current.LastBackupDate??DateTime.MinValue;
            if(last == DateTime.MinValue)
            {
                SettingsManager.Current.LastBackupDate = DateTime.Now;
                return false;
            }

            return  (DateTime.Now - last).TotalDays >= SettingsManager.Current.BackupRemainderDays; 
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
