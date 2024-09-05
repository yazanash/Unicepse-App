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
                string? uid = _viewModelBase.UID;
                player.Player? player = _playersDataStore.Players.FirstOrDefault(x => x.UID == uid);
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
                    await _playersAttendenceStore.LogInPlayer(dailyPlayerReport);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
