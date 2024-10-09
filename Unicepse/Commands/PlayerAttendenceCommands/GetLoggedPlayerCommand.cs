﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Commands;
using Unicepse.Stores;
using Unicepse.utlis.common;

namespace Unicepse.Commands.PlayerAttendenceCommands
{
    public class GetLoggedPlayerCommand : AsyncCommandBase
    {
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly HomeViewModel _homeViewModel;
        public GetLoggedPlayerCommand(PlayersAttendenceStore playersAttendenceStore, HomeViewModel homeViewModel)
        {
            _playersAttendenceStore = playersAttendenceStore;
            _homeViewModel = homeViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {

            await _playersAttendenceStore.GetLoggedPlayers(_homeViewModel.SelectedDate);
        }
    }
}
