﻿using Unicepse.Core.Models.DailyActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.Commands;
using System.Windows.Navigation;
using Unicepse.utlis.common;
using Unicepse.navigation;

namespace Unicepse.Commands.PlayerAttendenceCommands
{
    public class LoginPlayerCommand : AsyncCommandBase
    {
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly PlayersDataStore _playersDataStore;
        private NavigationService<HomeViewModel> _navigationService;
        public LoginPlayerCommand(PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playersDataStore,NavigationService<HomeViewModel> navigationService)
        {
            _playersAttendenceStore = playersAttendenceStore;
            _playersDataStore = playersDataStore;
            _navigationService = navigationService;
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
                _navigationService.ReNavigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
