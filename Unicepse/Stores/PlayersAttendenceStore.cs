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

namespace Unicepse.Stores
{
    public class PlayersAttendenceStore
    {
        public event Action<DailyPlayerReport>? LoggedIn;
        public event Action? Loaded;
        public event Action? PlayerLoggingLoaded;
        //public event Action<DailyPlayerReport>? Updated;
        public event Action<DailyPlayerReport>? LoggedOut;
        private readonly Lazy<Task> _initializeLazy;
        public PlayersAttendenceStore(PlayersAttendenceService playersAttendenceService, AttendanceApiDataService playersAttendenceApiService)
        {
            _playersAttendenceService = playersAttendenceService;
            _playersAttendence = new List<DailyPlayerReport>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _playersAttendenceApiService = playersAttendenceApiService;
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
            entity.DataStatus = DataStatus.ToCreate;
            await _playersAttendenceService.LogInPlayer(entity);

            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                try
                {
                    int status = await _playersAttendenceApiService.Create(entity);
                    if (status == 201 || status == 409)
                    {
                        entity.DataStatus = DataStatus.Synced;
                        await _playersAttendenceService.Update(entity);
                    }
                }
                catch { }

            }

            _playersAttendence.Add(entity);
            LoggedIn?.Invoke(entity);
        }

        public async Task LogOutPlayer(DailyPlayerReport entity)
        {
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;
            bool loggedOut = await _playersAttendenceService.LogOutPlayer(entity);

            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                try
                {
                    int status = await _playersAttendenceApiService.Update(entity);
                    if (status == 200)
                    {
                        entity.DataStatus = DataStatus.Synced;
                        await _playersAttendenceService.Update(entity);
                    }
                }
                catch { }
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

        public async Task GetLoggedPlayers(DateTime date)
        {
            IEnumerable<DailyPlayerReport> LoggedPlayers = await _playersAttendenceService.GetLoggedPlayers(date);
            _playersAttendence.Clear();
            _playersAttendence.AddRange(LoggedPlayers);
            Loaded?.Invoke();
        }
        public async Task<DailyPlayerReport?> GetLoggedPlayer(DailyPlayerReport entity)
        {
            return await _playersAttendenceService.Get(entity);
        }
        public async Task GetPlayerLogging(int playerid)
        {
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
                int status = await _playersAttendenceApiService.Create(attendance);
                if (status==201||status==409)
                {
                    attendance.DataStatus = DataStatus.Synced;
                    await _playersAttendenceService.Update(attendance);
                }


            }
        }

        public async Task SyncAttendanceToUpdate()
        {
            IEnumerable<DailyPlayerReport> attendances = await _playersAttendenceService.GetByDataStatus(DataStatus.ToUpdate);
            foreach (DailyPlayerReport attendance in attendances)
            {
                int status = await _playersAttendenceApiService.Update(attendance);
                if (status==200)
                {
                    attendance.DataStatus = DataStatus.Synced;
                    await _playersAttendenceService.Update(attendance);
                }


            }
        }

    }
}
