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

        public LoginPlayerScanCommand(ReadPlayerQrCodeViewModel viewModelBase, PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playersDataStore)
        {
            _viewModelBase = viewModelBase;
            _playersAttendenceStore = playersAttendenceStore;
            _playersDataStore = playersDataStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                CameraReader cameraReader = new CameraReader();
                cameraReader.DataContext = _viewModelBase;
                cameraReader.ShowDialog();
                if (!string.IsNullOrEmpty(_viewModelBase.UID))
                {
                    string? uid = _viewModelBase.UID;
                    player.Player? player = await _playersDataStore.GetPlayerByUID(uid);
                    if (player != null)
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
                    _viewModelBase.UID = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
