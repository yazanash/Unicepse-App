using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Models;
using Unicepse.API.Services;
using Unicepse.BackgroundServices;
using Unicepse.Core.Common;
using Unicepse.Core.Models.SyncModel;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Core.Services;

namespace Unicepse.Stores.ApiDataStores
{
    public class RoutineApiDataStore : IApiDataStore<PlayerRoutine>
    {
        private readonly RoutineApiDataService _routineApiDataService;
        private readonly ILogger<RoutineDataStore> _logger; 
        private readonly IDataService<SyncObject> _syncStore;

        string LogFlag = "[RADS] ";

        public RoutineApiDataStore(RoutineApiDataService routineApiDataService, ILogger<RoutineDataStore> logger, IDataService<SyncObject> syncStore)
        {
            _routineApiDataService = routineApiDataService;
            _logger = logger;
            _syncStore = syncStore;
        }

        public async Task Create(PlayerRoutine entity)
        {
            RoutineDto routineDto = new RoutineDto();
            routineDto.FromRoutine(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToCreate,
                EntityType = DataType.Routine,
                ObjectData = JsonConvert.SerializeObject(routineDto)
            };
            await _syncStore.Create(syncObject);
        }
        public async Task Sync(SyncObject syncObject)
        {
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    RoutineDto? routineDto = JsonConvert.DeserializeObject<RoutineDto>(syncObject.ObjectData!);
                    if (routineDto != null)
                    {
                        if (syncObject.OperationType == DataStatus.ToCreate)
                        {
                            _logger.LogInformation(LogFlag + "add routine to api");

                            int status = await _routineApiDataService.Create(routineDto);
                            if (status == 201 || status == 409)
                            {
                                _logger.LogInformation(LogFlag + "routine synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                        else
                        {
                            _logger.LogInformation(LogFlag + "update routine to api");

                            int status = await _routineApiDataService.Update(routineDto);
                            if (status == 200)
                            {
                                _logger.LogInformation(LogFlag + "routine synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "routine synced failed with error {0}", ex.Message);
                }
            }
        }


        public async Task Update(PlayerRoutine entity)
        {
            RoutineDto routineDto = new RoutineDto();
            routineDto.FromRoutine(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToUpdate,
                EntityType = DataType.Routine,
                ObjectData = JsonConvert.SerializeObject(routineDto)
            };
            await _syncStore.Create(syncObject);
        }
    }
}
