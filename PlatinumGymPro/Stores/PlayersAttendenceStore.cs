﻿using PlatinumGym.Core.Models.DailyActivity;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class PlayersAttendenceStore
    {
        public event Action<DailyPlayerReport>? LoggedIn;
        public event Action? Loaded;
        public event Action? PlayerLoggingLoaded;
        public event Action<DailyPlayerReport>? Updated;
        public event Action<int>? LoggedOut;
        private readonly Lazy<Task> _initializeLazy;
        public PlayersAttendenceStore(PlayersAttendenceService playersAttendenceService)
        {
            _playersAttendenceService = playersAttendenceService;
            _playersAttendence = new List<DailyPlayerReport>();
            _initializeLazy = new Lazy<Task>(Initialize);
        }

        private readonly PlayersAttendenceService _playersAttendenceService;
        private readonly List<DailyPlayerReport> _playersAttendence;
        public IEnumerable<DailyPlayerReport> PlayersAttendence => _playersAttendence;


        private DailyPlayerReport? _selectedDailyPlayerReport;
        public DailyPlayerReport? SelectedDailyPlayerReport
        {
            get
            {
                return _selectedDailyPlayerReport;
            }
            set
            {
                _selectedDailyPlayerReport = value;
            }
        }


        public async Task LogInPlayer(DailyPlayerReport entity)
        {
            await _playersAttendenceService.LogInPlayer(entity);
            _playersAttendence.Add(entity);
            LoggedIn?.Invoke(entity);
        }

        public async Task LogOutPlayer(DailyPlayerReport entity)
        {
            bool loggedOut = await _playersAttendenceService.LogOutPlayer(entity);
            int currentIndex = _playersAttendence.FindIndex(y => y.Id == entity.Id);
            _playersAttendence.RemoveAt(currentIndex);
            LoggedOut?.Invoke(entity.Id);
        }

        public async Task GetLoggedPlayers()
        {
            await _initializeLazy.Value;
            Loaded?.Invoke();
        }
        public async Task GetPlayerLogging(int playerid)
        {
            IEnumerable<DailyPlayerReport> subscriptions = await _playersAttendenceService.GetPlayerLogging(playerid);
            _playersAttendence.Clear();
            _playersAttendence.AddRange(subscriptions);
            PlayerLoggingLoaded?.Invoke();
        }
        public async Task Initialize()
        {
            IEnumerable<DailyPlayerReport> LoggedPlayers = await _playersAttendenceService.GetLoggedPlayers();
            _playersAttendence.Clear();
            _playersAttendence.AddRange(LoggedPlayers);
          
        }
      
    }
}