using Unicepse.Core.Models.DailyActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.Commands;

namespace Unicepse.Commands.PlayerAttendenceCommands
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
            if (_playersAttendenceStore.SelectedDailyPlayerReport != null)
            {
                _playersAttendenceStore.SelectedDailyPlayerReport!.logoutTime = DateTime.Now;
                _playersAttendenceStore.SelectedDailyPlayerReport!.IsLogged = false;
                await _playersAttendenceStore.LogOutPlayer(_playersAttendenceStore.SelectedDailyPlayerReport!);
            }

        }
    }
}
