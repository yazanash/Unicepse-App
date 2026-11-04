using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uniceps.API.Requests;
using Uniceps.API.Responses;
using Uniceps.API.SyncServices;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Sport;
using Uniceps.Entityframework.Services.DataSyncServices;

namespace Uniceps.BackgroundServices.SyncServices
{
    public class EmployeeSyncEngine : ISyncEngine
    {
        private readonly IPushPullDataService<Employee> _syncDataService;
        private readonly IServerChanges<EmployeeRequest, EmployeeResponse> _syncApiDataService;

        public EmployeeSyncEngine(IPushPullDataService<Employee> syncDataService, IServerChanges<EmployeeRequest, EmployeeResponse> syncApiDataService)
        {
            _syncDataService = syncDataService;
            _syncApiDataService = syncApiDataService;
        }

        public int Priority => 2;

        public string Name => "Employee";

        public async Task SyncAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<Employee> sports = await _syncDataService.GetDataToPush();
            await _syncApiDataService.PushLocalChangesAsync(sports.Select(x => new EmployeeRequest(x)).ToList());
            await _syncDataService.MarkAsSynced(sports.ToList());
            long latestVersion = await _syncDataService.GetLatestVersion();
            IEnumerable<EmployeeResponse> response = await _syncApiDataService.PulServerChangesAsync(latestVersion);
            List<Employee> pulledSports = response.Select(x => x.ToModel()).ToList();
            await _syncDataService.SaveDataFromPullAsync(pulledSports);
        }
    }
}
