using Unicepse.Core.Models.DailyActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.Commands;

namespace Unicepse.Commands.PlayerAttendenceCommands
{
    public class LoginPlayerCommand : AsyncCommandBase
    {
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly PlayersDataStore _playersDataStore;

        public LoginPlayerCommand(PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playersDataStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
            _playersDataStore = playersDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                //DailyPlayerReport ExistdailyPlayerReport = _playersAttendenceStore.get
                DailyPlayerReport dailyPlayerReport = new DailyPlayerReport()
                {
                    loginTime = DateTime.Now,
                    logoutTime = DateTime.Now,
                    Date = DateTime.Now,
                    IsLogged = true,
                    Player = _playersDataStore.SelectedPlayer!.Player,

                };
                await _playersAttendenceStore.LogInPlayer(dailyPlayerReport);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
