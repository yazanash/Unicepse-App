using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores;

namespace Uniceps.Commands.PlayerAttendenceCommands
{
    public class GetPlayerLoggingCommand : AsyncCommandBase
    {
        private readonly PlayersAttendenceStore _playersAttendenceStore;

        public GetPlayerLoggingCommand(PlayersAttendenceStore playersAttendenceStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if(parameter is int playerId)
            await _playersAttendenceStore.GetPlayerLogging(playerId);
        }
    }
}
