using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Models;
using Uniceps.API.Services;
using Uniceps.BackgroundServices;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Core.Services;
using Uniceps.Stores;

namespace Uniceps.Stores.ApiDataStores
{
    public class PaymentsApiDataStore : IApiDataStore<PlayerPayment>, IDeleteApiDataStore<PlayerPayment>
    {
        private readonly PaymentApiDataService _paymentApiDataService;
        private readonly IDataService<SyncObject> _syncStore;
        private readonly ILogger<PaymentDataStore> _logger;
        string LogFlag = "[Payments] ";

        public PaymentsApiDataStore(PaymentApiDataService paymentApiDataService, ILogger<PaymentDataStore> logger, IDataService<PlayerPayment> paymentDataService, IDataService<SyncObject> syncStore)
        {
            _paymentApiDataService = paymentApiDataService;
            _logger = logger;
            _syncStore = syncStore;
        }

        public async Task Create(PlayerPayment entity)
        {
            PaymentDto paymentDto = new PaymentDto();
            paymentDto.FromPayment(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToCreate,
                EntityType = DataType.Payment,
                ObjectData = JsonConvert.SerializeObject(paymentDto)
            };
            await _syncStore.Create(syncObject);
        }

        public async Task Delete(PlayerPayment entity)
        {
            PaymentDto paymentDto = new PaymentDto();
            paymentDto.FromPayment(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToDelete,
                EntityType = DataType.Payment,
                ObjectData = JsonConvert.SerializeObject(paymentDto)
            };
            await _syncStore.Create(syncObject);
        }
        public async Task Update(PlayerPayment entity)
        {
            PaymentDto paymentDto = new PaymentDto();
            paymentDto.FromPayment(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToUpdate,
                EntityType = DataType.Payment,
                ObjectData = JsonConvert.SerializeObject(paymentDto)
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
                    PaymentDto? paymentDto = JsonConvert.DeserializeObject<PaymentDto>(syncObject.ObjectData!);
                    if (paymentDto != null)
                    {
                        if (syncObject.OperationType == DataStatus.ToCreate)
                        {
                            _logger.LogInformation(LogFlag + "add payment to api");

                            int status = await _paymentApiDataService.Create(paymentDto);
                            if (status == 201 || status == 409)
                            {
                                _logger.LogInformation(LogFlag + "payment synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                        else if (syncObject.OperationType == DataStatus.ToUpdate)
                        {
                            _logger.LogInformation(LogFlag + "update payment to api");

                            int status = await _paymentApiDataService.Update(paymentDto);
                            if (status == 200)
                            {
                                _logger.LogInformation(LogFlag + "payment synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                        else if (syncObject.OperationType == DataStatus.ToDelete)
                        {
                            _logger.LogInformation(LogFlag + "delete payment from api");

                            int status = await _paymentApiDataService.Delete(paymentDto);
                            if (status == 200 || status == 404)
                            {
                                _logger.LogInformation(LogFlag + "payment synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "payment synced failed with error {0}", ex.Message);
                }
            }
        }
    }
}
