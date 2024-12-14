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
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Models.SyncModel;
using Unicepse.Core.Services;

namespace Unicepse.Stores.ApiDataStores
{
    public class SubscriptionApiDataStore : IApiDataStore<Subscription>
    {
        private readonly SubscriptionApiDataService _subscriptionApiDataService;
        private readonly ILogger<IApiDataStore<Subscription>> _logger;
        private readonly IDataService<SyncObject> _syncStore;
        string LogFlag = "[SADS] ";

        public SubscriptionApiDataStore(SubscriptionApiDataService subscriptionApiDataService, ILogger<IApiDataStore<Subscription>> logger, IDataService<SyncObject> syncStore)
        {
            _subscriptionApiDataService = subscriptionApiDataService;
            _logger = logger;
            _syncStore = syncStore;
        }

        public async Task Create(Subscription entity)
        {
            SubscriptionDto subscriptionDto = new SubscriptionDto();
            subscriptionDto.FromSubscription(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToCreate,
                EntityType = DataType.Subscription,
                ObjectData = JsonConvert.SerializeObject(subscriptionDto)
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
                    SubscriptionDto? subscriptionDto = JsonConvert.DeserializeObject<SubscriptionDto>(syncObject.ObjectData!);
                    if (subscriptionDto != null)
                    {
                        if (syncObject.OperationType == DataStatus.ToCreate)
                        {
                            _logger.LogInformation(LogFlag + "add Subscription to api");

                            int status = await _subscriptionApiDataService.Create(subscriptionDto);
                            if (status == 201 || status == 409)
                            {
                                _logger.LogInformation(LogFlag + "Subscription synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                        else
                        {
                            _logger.LogInformation(LogFlag + "update Subscription to api");

                            int status = await _subscriptionApiDataService.Update(subscriptionDto);
                            if (status == 200)
                            {
                                _logger.LogInformation(LogFlag + "Subscription synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "Subscription synced failed with error {0}", ex.Message);
                }
            }
        }

        public async Task Update(Subscription entity)
        {
            SubscriptionDto subscriptionDto = new SubscriptionDto();
            subscriptionDto.FromSubscription(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToUpdate,
                EntityType = DataType.Subscription,
                ObjectData = JsonConvert.SerializeObject(subscriptionDto)
            };
            await _syncStore.Create(syncObject);
        }
    }
}
