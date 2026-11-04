using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Requests;
using Uniceps.API.Responses;

namespace Uniceps.API.SyncServices
{
    public class SportSyncService : IServerChanges<SportRequest, SportResponse>
    {
        private readonly UnicepseApiPrepHttpClient _apiClient;

        public SportSyncService(UnicepseApiPrepHttpClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<IEnumerable<SportResponse>> PulServerChangesAsync(long lastVersion)
        {
            var response = await _apiClient.GetAsync<IEnumerable<SportResponse>>($"api/sports?lastServerVersion={lastVersion}");

            if (response.StatusCode == 200 && response.Data != null)
            {
                return response.Data;
            }

            return Enumerable.Empty<SportResponse>();
        }

        public async Task PushLocalChangesAsync(List<SportRequest> data)
        {
            await _apiClient.PostAsync<List<SportRequest>, bool>("api/sports", data);
        }
    }
}
