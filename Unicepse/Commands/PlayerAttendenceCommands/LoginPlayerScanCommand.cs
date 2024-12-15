using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Core.Models.DailyActivity;
using Unicepse.navigation;
using Unicepse.navigation.Stores;
using Unicepse.Stores;
using Unicepse.Stores.RoutineStores;
using Unicepse.ViewModels.PlayersViewModels;
using player = Unicepse.Core.Models.Player;
namespace Unicepse.Commands.PlayerAttendenceCommands
{
    public class LoginPlayerScanCommand : AsyncCommandBase
    {
        private readonly ReadPlayerQrCodeViewModel _viewModelBase;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly SportDataStore _sportDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly MetricDataStore _metricDataStore;
        private readonly RoutineDataStore _routineDataStore;
        private readonly LicenseDataStore _licenseDataStore;
        private readonly NavigationService<PlayerListViewModel> _navigationService;
        private readonly ExercisesDataStore? _exercisesDataStore;

        private readonly RoutineTemplatesDataStore? _routineTemplatesDataStore;
        public string? OldUID;

        public LoginPlayerScanCommand(ReadPlayerQrCodeViewModel viewModelBase, PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, LicenseDataStore licenseDataStore, NavigationService<PlayerListViewModel> navigationService, ExercisesDataStore exercisesDataStore, RoutineTemplatesDataStore routineTemplatesDataStore)
        {
            _viewModelBase = viewModelBase;
            _playersAttendenceStore = playersAttendenceStore;
            _playersDataStore = playersDataStore;
            _navigationStore = navigationStore;
            _subscriptionDataStore = subscriptionDataStore;
            _sportDataStore = sportDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _licenseDataStore = licenseDataStore;
            _navigationService = navigationService;
            _exercisesDataStore = exercisesDataStore;
            _routineTemplatesDataStore = routineTemplatesDataStore;

            _viewModelBase.onCatch += _viewModelBase_onCatch;
        }

        private async void _viewModelBase_onCatch()
        {
            try
            {

                if (!string.IsNullOrEmpty(_viewModelBase.UID)&& _viewModelBase.UID!=OldUID)
                {
                    OldUID = _viewModelBase.UID;
                    string? uid = _viewModelBase.UID;
                    player.Player? player = await _playersDataStore.GetPlayerByUID(uid);
                    if (player != null)
                    {
                        if (player.SubscribeEndDate > DateTime.Now)
                        {
                            DailyPlayerReport dailyPlayerReport = new DailyPlayerReport()
                            {
                                loginTime = DateTime.Now,
                                logoutTime = DateTime.Now,
                                Date = DateTime.Now,
                                IsLogged = true,
                                Player = player

                            };
                            DailyPlayerReport? existed = await _playersAttendenceStore.GetLoggedPlayer(dailyPlayerReport);
                            if (existed != null)
                            {
                                existed.logoutTime = DateTime.Now;
                                existed.IsLogged = false;
                                await _playersAttendenceStore.LogOutPlayer(existed);
                            }
                            else
                                await _playersAttendenceStore.LogInPlayer(dailyPlayerReport);
                        }
                        else
                        {
                            if (MessageBox.Show("هذا اللاعب منتهي الاشتراك هل تريد اضافة اشتراك له ؟", "تنبيه", MessageBoxButton.YesNo,
                                           MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                _playersDataStore.SelectedPlayer = player;
                                NavigationStore PlayerMainPageNavigation = new NavigationStore();
                                NavigationService<PlayerProfileViewModel> nav = new NavigationService<PlayerProfileViewModel>(_navigationStore, () => CreatePlayerProfileViewModel(player, PlayerMainPageNavigation, _subscriptionDataStore, _playersDataStore, _sportDataStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore, _licenseDataStore, _navigationService, _exercisesDataStore!, _routineTemplatesDataStore!));
                                nav.Navigate();
                            }
                                
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void _viewModelBase_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_viewModelBase.UID))
                {
                    string? uid = _viewModelBase.UID;
                    player.Player? player = await _playersDataStore.GetPlayerByUID(uid);
                    if (player != null)
                    {
                        if(player.SubscribeEndDate > DateTime.Now)
                        {
                            DailyPlayerReport dailyPlayerReport = new DailyPlayerReport()
                            {
                                loginTime = DateTime.Now,
                                logoutTime = DateTime.Now,
                                Date = DateTime.Now,
                                IsLogged = true,
                                Player = player

                            };
                            DailyPlayerReport? existed = await _playersAttendenceStore.GetLoggedPlayer(dailyPlayerReport);
                            if (existed != null)
                            {
                                existed.logoutTime = DateTime.Now;
                                existed.IsLogged = false;
                                await _playersAttendenceStore.LogOutPlayer(existed);
                            }
                            else
                                await _playersAttendenceStore.LogInPlayer(dailyPlayerReport);
                        }
                        else
                        {
                            MessageBox.Show("هذا اللاعب منتهي الاشتراك");
                        }
                    }
                    _viewModelBase.UID = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is CameraReader)
                    {
                        window.Close();
                    }
                }
                CameraReader cameraReader = new CameraReader(true,_viewModelBase);
                cameraReader.DataContext = _viewModelBase;
                cameraReader.Show();
                await Task.Delay(1);
                _viewModelBase.UID = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static PlayerProfileViewModel CreatePlayerProfileViewModel(player.Player player, NavigationStore navigatorStore,
           SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, SportDataStore sportDataStore,
           PaymentDataStore paymentDataStore, MetricDataStore _metricDataStore, RoutineDataStore routineDataStore,
           PlayersAttendenceStore playersAttendenceStore, LicenseDataStore licenseDataStore, NavigationService<PlayerListViewModel> navigationService,
           ExercisesDataStore exercisesDataStore, RoutineTemplatesDataStore routineTemplatesDataStore)
        {

            playersDataStore.SelectedPlayer = player;
            return new PlayerProfileViewModel(navigatorStore, subscriptionDataStore, playersDataStore, sportDataStore,
                paymentDataStore, _metricDataStore, routineDataStore, playersAttendenceStore, licenseDataStore, navigationService,
                exercisesDataStore, routineTemplatesDataStore);
        }
    }
}
