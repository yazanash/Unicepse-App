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
using Unicepse.Core.Models.Metric;
using Unicepse.Core.Models.SyncModel;
using Unicepse.Core.Services;

namespace Unicepse.Stores.ApiDataStores
{
    public class MetricApiDataStore : IApiDataStore<Metric>
    {
        private readonly MetricApiDataService _metricApiDataService;
        private readonly IDataService<SyncObject> _syncStore;
        private readonly ILogger<IApiDataStore<Metric>> _logger;
        string LogFlag = "[Metrics] ";

        public MetricApiDataStore(MetricApiDataService metricApiDataService, ILogger<IApiDataStore<Metric>> logger, IDataService<SyncObject> syncStore)
        {
            _metricApiDataService = metricApiDataService;
            _logger = logger;
            _syncStore = syncStore;
        }

        public async Task Create(Metric entity)
        {
            MetricDto metricDto = new MetricDto();
            metricDto.FromMetric(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToCreate,
                EntityType = DataType.Metric,
                ObjectData = JsonConvert.SerializeObject(metricDto)
            };
            await _syncStore.Create(syncObject);
        }

       

        public async Task Update(Metric entity)
        {
            MetricDto metricDto = new MetricDto();
            metricDto.FromMetric(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToUpdate,
                EntityType = DataType.Metric,
                ObjectData = JsonConvert.SerializeObject(metricDto)
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
                    MetricDto? metricDto = JsonConvert.DeserializeObject<MetricDto>(syncObject.ObjectData!);
                    if (metricDto != null)
                    {
                        if (syncObject.OperationType == DataStatus.ToCreate)
                        {
                            _logger.LogInformation(LogFlag + "add metric to api");

                            int status = await _metricApiDataService.Create(metricDto);
                            if (status == 201 || status == 409)
                            {
                                _logger.LogInformation(LogFlag + "metric synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                        else
                        {
                            _logger.LogInformation(LogFlag + "update metric to api");

                            int status = await _metricApiDataService.Update(metricDto);
                            if (status == 200)
                            {
                                _logger.LogInformation(LogFlag + "metric synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "metric synced failed with error {0}", ex.Message);
                }
            }
        }

    }
}
