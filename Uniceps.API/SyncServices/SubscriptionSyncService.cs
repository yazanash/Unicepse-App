using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Requests;
using Uniceps.API.Responses;

namespace Uniceps.API.SyncServices
{
    public class SubscriptionSyncService : IServerChanges<SubscriptionRequest, SubscriptionResponse>
    {
        private readonly HttpClient _httpClient;

        public SubscriptionSyncService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://192.168.1.9:5800/");
        }
        public async Task<IEnumerable<SubscriptionResponse>> PulServerChangesAsync(long data)
        {
            IEnumerable<SubscriptionResponse>? Data = await _httpClient.GetFromJsonAsync<IEnumerable<SubscriptionResponse>>($"api/subscriptions?lastServerVersion={data}");
            return Data ?? new List<SubscriptionResponse>();
        }

        public async Task PushLocalChangesAsync(List<SubscriptionRequest> data)
        {
            var response = await _httpClient.PostAsJsonAsync("api/subscriptions", data);
        }
    }
}
