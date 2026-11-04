using System;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Helpers;
using Uniceps.navigation.NavigationSystem;
using Uniceps.navigation.Navigator;
using Uniceps.Stores;
using Uniceps.SystemServices;
using Uniceps.utlis.ComponentsViewModels;
using Uniceps.Views;
namespace Uniceps.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public INavigator Navigator { get; set; }
        private readonly NavigationStore _navigationStore;
        private readonly LicenseStore _licenseStore;
        SystemSubscriptionPaymentOptionDialogViewModel _systemSubscription;
        public StatusBarViewModel StatusBarViewModel { get; set; }
        public NotificationBarViewModel NotificationBarViewModel { get; set; }
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

        public MainWindowViewModel(INavigator navigator, LicenseStore licenseStore, SystemSubscriptionPaymentOptionDialogViewModel systemSubscription, StatusBarViewModel statusBarViewModel, NavigationStore navigationStore)
        {
            Navigator = navigator;
            _systemSubscription = systemSubscription;
            NotificationBarViewModel = new NotificationBarViewModel();
            var savedTheme = SettingsManager.Current.AppTheme;
            if (Enum.TryParse(savedTheme, out AppTheme theme))
            {
                CurrentTheme = theme;
            }
            if (IsBackupNeeded())
            {
                NotificationManager.SendBackupNeededNotification();
            }
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

            StatusBarViewModel = statusBarViewModel;
            _navigationStore = navigationStore;
            CurrentViewModel = _navigationStore.CurrentViewModel;
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
        }
        private void _navigationStore_CurrentViewModelChanged()
        {
            CurrentViewModel = _navigationStore.CurrentViewModel;
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private ViewModelBase? _CurrentViewModel;

        public ViewModelBase? CurrentViewModel
        {
            get { return _CurrentViewModel!; }
            set { _CurrentViewModel = value; OnPropertyChanged(nameof(CurrentViewModel)); }

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
            DateTime last = SettingsManager.Current.LastBackupDate ?? DateTime.MinValue;
            if (last == DateTime.MinValue)
            {
                SettingsManager.Current.LastBackupDate = DateTime.Now;
                return false;
            }

            return (DateTime.Now - last).TotalDays >= SettingsManager.Current.BackupRemainderDays;
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

    }
}
