﻿using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.PlayerAttendenceCommands
{
    public class GetLoggedPlayerCommand : AsyncCommandBase
    {
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        public GetLoggedPlayerCommand(PlayersAttendenceStore playersAttendenceStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
          
            await _playersAttendenceStore.GetLoggedPlayers();
        }
    }
}
