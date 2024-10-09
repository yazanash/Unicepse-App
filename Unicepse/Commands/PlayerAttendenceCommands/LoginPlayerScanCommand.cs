using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Core.Models.DailyActivity;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersViewModels;
using player = Unicepse.Core.Models.Player;
namespace Unicepse.Commands.PlayerAttendenceCommands
{
    public class LoginPlayerScanCommand : AsyncCommandBase
    {
        private readonly ReadPlayerQrCodeViewModel _viewModelBase;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly PlayersDataStore _playersDataStore;
        public string? OldUID;

        public LoginPlayerScanCommand(ReadPlayerQrCodeViewModel viewModelBase, PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playersDataStore)
        {
            _viewModelBase = viewModelBase;
            _playersAttendenceStore = playersAttendenceStore;
            _playersDataStore = playersDataStore;
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
                            MessageBox.Show("هذا اللاعب منتهي الاشتراك");
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
                CameraReader cameraReader = new CameraReader(true,_viewModelBase);
                cameraReader.DataContext = _viewModelBase;
                cameraReader.ShowDialog();
                await Task.Delay(1);
                _viewModelBase.UID = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
