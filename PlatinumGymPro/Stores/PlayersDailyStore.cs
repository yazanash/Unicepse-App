using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class PlayersDailyStore
    {
        private readonly PlayersTrafficService playersTrafficService;
        private readonly List<DailyPlayerReport> playersReports;
        public IEnumerable<DailyPlayerReport> PlayersReports => playersReports;
        public PlayersDailyStore(PlayersTrafficService playersTrafficService)
        {
            this.playersTrafficService = playersTrafficService;
            this.playersReports = new List<DailyPlayerReport>();
        }
        public event Action? ReportLoaded;
        public event Action? ActivePlayerReportLoaded;
        public event Action<DailyPlayerReport>? ReportAdded;
        public event Action<DailyPlayerReport>? ReportUpdated;
        public event Action<int>? ReportDeleted;

        public async Task Load()
        {
            IEnumerable<DailyPlayerReport> sports = await playersTrafficService.GetAll();

            playersReports.Clear();
            playersReports.AddRange(sports);

            ReportLoaded?.Invoke();
        }
        public async Task LoadActivePlayerReport()
        {
            IEnumerable<DailyPlayerReport> sports = await playersTrafficService.GetActivatedPlayers();

            playersReports.Clear();
            playersReports.AddRange(sports);

            ReportLoaded?.Invoke();
        }
        public async Task Add(DailyPlayerReport dailyPlayerReport)
        {
            await playersTrafficService.Create(dailyPlayerReport);
            playersReports.Add(dailyPlayerReport);

            ReportAdded?.Invoke(dailyPlayerReport);
        }
        public async Task Update(DailyPlayerReport dailyPlayerReport)
        {
            await playersTrafficService.Update(dailyPlayerReport);

            int currentIndex = playersReports.FindIndex(y => y.Id == dailyPlayerReport.Id);

            if (currentIndex != -1)
            {
                playersReports[currentIndex] = dailyPlayerReport;
            }
            else
            {
                playersReports.Add(dailyPlayerReport);
            }

            ReportUpdated?.Invoke(dailyPlayerReport);
        }
        public async Task Delete(int id)
        {
            await playersTrafficService.Delete(id);

            playersReports.RemoveAll(y => y.Id == id);

            ReportDeleted?.Invoke(id);
        }

    }
}
