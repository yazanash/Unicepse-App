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
        private readonly Lazy<Task> _initializeLazy;
        public PlayersAttendenceStore(PlayersAttendenceService playersAttendenceService, AttendanceApiDataService playersAttendenceApiService, ILogger<PlayersAttendenceStore> logger)
        {
            _playersAttendenceService = playersAttendenceService;
            _playersAttendence = new List<DailyPlayerReport>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _playersAttendenceApiService = playersAttendenceApiService;
            _logger = logger;
        }

        private readonly PlayersAttendenceService _playersAttendenceService;
        private readonly AttendanceApiDataService _playersAttendenceApiService;
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

            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "add attendance to api");
                    int status = await _playersAttendenceApiService.Create(entity);
                    if (status == 201 || status == 409)
                    {
                        _logger.LogInformation(LogFlag + "attendance synced successfully with code {0}", status.ToString());
                        entity.DataStatus = DataStatus.Synced;
                        await _playersAttendenceService.UpdateDataStatus(entity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "attendance synced failed with error {0}", ex.Message);
                }

            }

            _playersAttendence.Add(entity);
            LoggedIn?.Invoke(entity);
        }

        public async Task LogOutPlayer(DailyPlayerReport entity)
        {
            _logger.LogInformation(LogFlag + "logout player");
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;
            bool loggedOut = await _playersAttendenceService.LogOutPlayer(entity);

            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "update attendance to api");
                    int status = await _playersAttendenceApiService.Update(entity);
                    if (status == 200)
                    {
                        _logger.LogInformation(LogFlag + "attendance synced successfully with code {0}", status.ToString());
                        entity.DataStatus = DataStatus.Synced;
                        await _playersAttendenceService.UpdateDataStatus(entity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "attendance synced failed with error {0}", ex.Message);
                }
            }
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
        public async Task SyncAttendanceToCreate()
        {
            IEnumerable<DailyPlayerReport> attendances = await _playersAttendenceService.GetByDataStatus(DataStatus.ToCreate);
            foreach (DailyPlayerReport attendance in attendances)
            {
                _logger.LogInformation(LogFlag + "create attendance to api");
                int status = await _playersAttendenceApiService.Create(attendance);
                if (status==201||status==409)
                {
                    _logger.LogInformation(LogFlag + "attendance synced successfully with code {0}", status.ToString());
                    attendance.DataStatus = DataStatus.Synced;
                    await _playersAttendenceService.UpdateDataStatus(attendance);
                }
                else
                {
                    _logger.LogWarning(LogFlag + "attendance synced failed with code {0}", status.ToString());
                }


            }
        }

        public async Task SyncAttendanceToUpdate()
        {
            IEnumerable<DailyPlayerReport> attendances = await _playersAttendenceService.GetByDataStatus(DataStatus.ToUpdate);
            foreach (DailyPlayerReport attendance in attendances)
            {
                _logger.LogInformation(LogFlag + "update attendance to api");
                int status = await _playersAttendenceApiService.Update(attendance);
                if (status==200)
                {
                    _logger.LogInformation(LogFlag + "attendance synced successfully with code {0}", status.ToString());
                    attendance.DataStatus = DataStatus.Synced;
                    await _playersAttendenceService.UpdateDataStatus(attendance);
                }
                else
                {
                    _logger.LogWarning(LogFlag + "attendance synced failed with code {0}", status.ToString());
                }


            }
        }

    }
}
