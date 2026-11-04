using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uniceps.API.Requests;
using Uniceps.API.Responses;
using Uniceps.API.SyncServices;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Entityframework.Services.DataSyncServices;

namespace Uniceps.BackgroundServices.SyncServices
{
    public class SportSyncEngine : ISyncEngine
    {
        private readonly IPushPullDataService<Sport> _sportSyncDataService;
        private readonly IServerChanges<SportRequest, SportResponse> _sportSyncApiDataService;


        public SportSyncEngine(IPushPullDataService<Sport> sportSyncDataService, IServerChanges<SportRequest, SportResponse> sportSyncApiDataService)
        {
            _sportSyncDataService = sportSyncDataService;
            _sportSyncApiDataService = sportSyncApiDataService;
        }

        public int Priority => 1;

        public string Name => "Sport Engine";

        public async Task SyncAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<Sport> sports = await _sportSyncDataService.GetDataToPush();
            await _sportSyncApiDataService.PushLocalChangesAsync(sports.Select(x => new SportRequest(x)).ToList());
            await _sportSyncDataService.MarkAsSynced(sports.ToList());
            long latestVersion = await _sportSyncDataService.GetLatestVersion();
            IEnumerable<SportResponse> response = await _sportSyncApiDataService.PulServerChangesAsync(latestVersion);
            List<Sport> pulledSports = response.Select(x => x.ToModel()).ToList();
            await _sportSyncDataService.SaveDataFromPullAsync(pulledSports);
        }
    }
}
