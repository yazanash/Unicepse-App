using PlatinumGym.Core.Models.DailyActivity;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.PlayerAttendenceCommands
{
    public class LogoutPlayerCommand : AsyncCommandBase
    {
        private readonly PlayersAttendenceStore _playersAttendenceStore;

        public LogoutPlayerCommand(PlayersAttendenceStore playersAttendenceStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            DailyPlayerReport dailyPlayerReport = new DailyPlayerReport()
            {
                Id=_playersAttendenceStore.SelectedDailyPlayerReport!.Id,
                logoutTime = DateTime.Now,
                IsLogged = false,
            };
            await _playersAttendenceStore.LogOutPlayer(dailyPlayerReport);
        }
    }
}
