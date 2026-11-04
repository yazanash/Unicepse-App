using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Requests;
using Uniceps.API.Responses;
using Uniceps.Core.Models.Player;

namespace Uniceps.API.SyncServices
{
    public class PlayersSyncService : IServerChanges<PushPlayerRequest,PullPlayerResponse>
    {
        private readonly UnicepseApiPrepHttpClient _apiClient;

        public PlayersSyncService(UnicepseApiPrepHttpClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<IEnumerable<PullPlayerResponse>> PulServerChangesAsync(long lastVersion)
        {
            var response = await _apiClient.GetAsync<IEnumerable<PullPlayerResponse>>($"api/players?lastServerVersion={lastVersion}");

            if (response.StatusCode == 200 && response.Data != null)
            {
                return response.Data;
            }

            return Enumerable.Empty<PullPlayerResponse>();
        }

        public async Task PushLocalChangesAsync(List<PushPlayerRequest> data)
        {
            await _apiClient.PostAsync<List<PushPlayerRequest>, bool>("api/players", data);
        }
    }
}
