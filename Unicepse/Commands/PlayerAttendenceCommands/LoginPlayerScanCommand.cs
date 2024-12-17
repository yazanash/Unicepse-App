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
        private readonly PlayerProfileViewModel _playerProfileViewModel;

        public string? OldUID;

        public LoginPlayerScanCommand(ReadPlayerQrCodeViewModel viewModelBase, PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, PlayerProfileViewModel playerProfileViewModel)
        {
            _viewModelBase = viewModelBase;
            _playersAttendenceStore = playersAttendenceStore;
            _playersDataStore = playersDataStore;
            _navigationStore = navigationStore;
            _playerProfileViewModel = playerProfileViewModel;

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
                                NavigationService<PlayerProfileViewModel> nav = new NavigationService<PlayerProfileViewModel>(_navigationStore, () => _playerProfileViewModel);
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
