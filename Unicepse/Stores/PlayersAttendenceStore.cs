using Unicepse.Core.Models.DailyActivity;
using Unicepse.Core.Models.Player;
using Unicepse.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Services;
using Unicepse.Core.Common;
using Unicepse.BackgroundServices;
using Microsoft.Extensions.Logging;
using Unicepse.Core.Services;
using Unicepse.Stores.ApiDataStores;

namespace Unicepse.Stores
{
    public class PlayersAttendenceStore
    {
        public event Action<DailyPlayerReport>? LoggedIn;
        public event Action? Loaded;
        public event Action? PlayerLoggingLoaded;
        string LogFlag = "[Attendence] ";
        private readonly ILogger<PlayersAttendenceStore> _logger;
        public event Action<DailyPlayerReport>? LoggedOut;
        private readonly Lazy<Task>? _initializeLazy;
        public PlayersAttendenceStore(PlayersAttendenceService playersAttendenceService, ILogger<PlayersAttendenceStore> logger, IApiDataStore<DailyPlayerReport> apiDataStore)
        {
            _playersAttendenceService = playersAttendenceService;
            _playersAttendence = new List<DailyPlayerReport>();
            //_initializeLazy = new Lazy<Task>(Initialize);
            _logger = logger;
            _apiDataStore = apiDataStore;
        }

        private readonly PlayersAttendenceService _playersAttendenceService;
        private readonly IApiDataStore<DailyPlayerReport> _apiDataStore;
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
            _logger.LogInformation(LogFlag + "login player");
            entity.DataStatus = DataStatus.ToCreate;
            await _playersAttendenceService.LogInPlayer(entity);
            await _apiDataStore.Create(entity);

            _playersAttendence.Add(entity);
            LoggedIn?.Invoke(entity);
        }

        public async Task LogOutPlayer(DailyPlayerReport entity)
        {
            _logger.LogInformation(LogFlag + "logout player");
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;
            bool loggedOut = await _playersAttendenceService.LogOutPlayer(entity);
            await _apiDataStore.Update(entity);

            int currentIndex = _playersAttendence.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _playersAttendence[currentIndex] = entity;
            }
            else
            {
                _playersAttendence.Add(entity);
            }
            LoggedOut?.Invoke(entity);
        }
        public async Task AddKeyToPlayer(DailyPlayerReport entity)
        {
            _logger.LogInformation(LogFlag + "add key to player");
            DailyPlayerReport loggedOut = await _playersAttendenceService.AddKey(entity);

            int currentIndex = _playersAttendence.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _playersAttendence[currentIndex] = entity;
            }
            else
            {
                _playersAttendence.Add(entity);
            }
            LoggedOut?.Invoke(entity);
        }
        public async Task GetLoggedPlayers(DateTime date)
        {
            _logger.LogInformation(LogFlag + "get logs history");
            IEnumerable<DailyPlayerReport> LoggedPlayers = await _playersAttendenceService.GetLoggedPlayers(date);
            _playersAttendence.Clear();
            _playersAttendence.AddRange(LoggedPlayers);
            Loaded?.Invoke();
        }
        public async Task<DailyPlayerReport?> GetLoggedPlayer(DailyPlayerReport entity)
        {
            _logger.LogInformation(LogFlag + "get player log");
            return await _playersAttendenceService.Get(entity);
        }
        public async Task GetPlayerLogging(int playerid)
        {
            _logger.LogInformation(LogFlag + "get player logs");
            IEnumerable<DailyPlayerReport> subscriptions = await _playersAttendenceService.GetPlayerLogging(playerid);
            _playersAttendence.Clear();
            _playersAttendence.AddRange(subscriptions);
            PlayerLoggingLoaded?.Invoke();
        }
        public Task Initialize()
        {
            throw new NotImplementedException();

        }

    }
}
