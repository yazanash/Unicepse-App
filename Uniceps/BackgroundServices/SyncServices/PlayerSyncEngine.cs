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
using Uniceps.Entityframework.Services.DataSyncServices;

namespace Uniceps.BackgroundServices.SyncServices
{
    public class PlayerSyncEngine : ISyncEngine
    {
       
        private readonly IPushPullDataService<Player> _playerSyncDataService;
        private readonly IServerChanges<PushPlayerRequest, PullPlayerResponse> _playerSyncApiDataService;
        public PlayerSyncEngine(IPushPullDataService<Player> playerSyncDataService, IServerChanges<PushPlayerRequest, PullPlayerResponse> playerSyncApiDataService)
        {
            _playerSyncDataService = playerSyncDataService;
            _playerSyncApiDataService = playerSyncApiDataService;
        }

        public int Priority =>2;

        public string Name => "Players";

        public async Task SyncAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<Player> players = await _playerSyncDataService.GetDataToPush();
            await _playerSyncApiDataService.PushLocalChangesAsync(players.Select(x => new PushPlayerRequest(x)).ToList());
            await _playerSyncDataService.MarkAsSynced(players.ToList());
            long latestVersion = await _playerSyncDataService.GetLatestVersion();
            IEnumerable<PullPlayerResponse> response = await _playerSyncApiDataService.PulServerChangesAsync(latestVersion);
            List<Player> pulledPlayers = response.Select(x => x.ToModel()).ToList();
            await _playerSyncDataService.SaveDataFromPullAsync(pulledPlayers);
        }
    }
}
