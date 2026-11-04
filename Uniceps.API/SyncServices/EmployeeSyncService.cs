using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Requests;
using Uniceps.API.Responses;

namespace Uniceps.API.SyncServices
{
    public class EmployeeSyncService : IServerChanges<EmployeeRequest, EmployeeResponse>
    {
        private readonly UnicepseApiPrepHttpClient _apiClient;

        public EmployeeSyncService(UnicepseApiPrepHttpClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<IEnumerable<EmployeeResponse>> PulServerChangesAsync(long lastVersion)
        {
            var response = await _apiClient.GetAsync<IEnumerable<EmployeeResponse>>($"api/employees?lastServerVersion={lastVersion}");

            if (response.StatusCode == 200 && response.Data != null)
            {
                return response.Data;
            }

            return Enumerable.Empty<EmployeeResponse>();
        }

        public async Task PushLocalChangesAsync(List<EmployeeRequest> data)
        {
            await _apiClient.PostAsync<List<EmployeeRequest>, bool>("api/employees", data);
        }
    }
}
