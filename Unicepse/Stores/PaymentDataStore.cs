using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.API.Services;
using Unicepse.Core.Common;
using Unicepse.BackgroundServices;
using Microsoft.Extensions.Logging;
using Unicepse.Core.Services;

namespace Unicepse.Stores
{
    public class PaymentDataStore
    {
        public event Action<PlayerPayment>? Created;
        public event Action? Loaded;
        public event Action<PlayerPayment>? Updated;
        public event Action<int>? Deleted;
        public event Action? SumUpdated;
        string LogFlag = "[Payments] ";
        private readonly ILogger<PaymentDataStore> _logger;
        public PaymentDataStore(IPaymentDataService paymentDataService, PaymentApiDataService paymentApiDataService, ILogger<PaymentDataStore> logger)
        {
            _paymentDataService = paymentDataService;
            _payments = new List<PlayerPayment>();
            _paymentApiDataService = paymentApiDataService;
            _logger = logger;
        }

        private readonly IPaymentDataService _paymentDataService;
        private readonly PaymentApiDataService _paymentApiDataService;
        private readonly List<PlayerPayment> _payments;
        public IEnumerable<PlayerPayment> Payments => _payments;




        private PlayerPayment? _selectedPayment;
        public PlayerPayment? SelectedPayment
        {
            get
            {
                return _selectedPayment;
            }
            set
            {
                _selectedPayment = value;
                _logger.LogInformation(LogFlag + "selected payments changed");
            }
        }




        public async Task Add(PlayerPayment entity)
        {
            _logger.LogInformation(LogFlag + "add payment");
            entity.DataStatus = DataStatus.ToCreate;
            await _paymentDataService.Create(entity);
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "add payment to api");
                    int status = await _paymentApiDataService.Create(entity);
                    if (status == 201 || status == 409)
                    {
                        _logger.LogInformation(LogFlag + "payment synced successfully with code {0}", status.ToString());
                        entity.DataStatus = DataStatus.Synced;
                        await _paymentDataService.UpdateDataStatus(entity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "payment synced failes with error {0}", ex.Message);
                }

            }
            _payments.Add(entity);
            Created?.Invoke(entity);
            SumUpdated?.Invoke();
        }


        public async Task Delete(PlayerPayment entity)
        {
            _logger.LogInformation(LogFlag + "delete payment");
            if (entity.DataStatus == DataStatus.ToCreate)
                await DeleteForce(entity.Id);
            else
            {
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    try
                    {
                        _logger.LogInformation(LogFlag + "delete payment from api");
                        int status = await _paymentApiDataService.Delete(entity);
                        if (status == 200 )
                        {
                            _logger.LogInformation(LogFlag + "payment synced successfully with code {0}", status.ToString());
                            await DeleteForce(entity.Id);
                        }
                        else
                        {
                            _logger.LogInformation(LogFlag + "payment synced failed with code {0}", status.ToString());
                            entity.DataStatus = DataStatus.ToDelete;
                            await _paymentDataService.UpdateDataStatus(entity);
                            int currentIndex = _payments.FindIndex(y => y.Id == entity.Id);
                            _payments.RemoveAt(currentIndex);
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(LogFlag + "payment synced failed with error {0}", ex.Message);
                        entity.DataStatus = DataStatus.ToDelete;
                        await _paymentDataService.UpdateDataStatus(entity);
                        int currentIndex = _payments.FindIndex(y => y.Id == entity.Id);
                        _payments.RemoveAt(currentIndex);
                    }

                }
                else
                {
                    _logger.LogError(LogFlag + "payment synced failed no internet available ");
                    entity.DataStatus = DataStatus.ToDelete;
                    await _paymentDataService.Update(entity);
                    int currentIndex = _payments.FindIndex(y => y.Id == entity.Id);
                    _payments.RemoveAt(currentIndex);
                }
             
            }
            Deleted?.Invoke(entity.Id);
            SumUpdated?.Invoke();
        }

        public async Task DeleteForce(int entity_id)
        {
            _logger.LogInformation(LogFlag + "force delete payment");
            bool deleted = await _paymentDataService.Delete(entity_id);
            int currentIndex = _payments.FindIndex(y => y.Id == entity_id);
            _payments.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
            SumUpdated?.Invoke();
        }

        public async Task GetPlayerPayments(Player player)
        {
            _logger.LogInformation(LogFlag + "get player payments");
            IEnumerable<PlayerPayment> subscriptions = await _paymentDataService.GetPlayerPayments(player);
            _payments.Clear();
            _payments.AddRange(subscriptions);
            Loaded?.Invoke();
            SumUpdated?.Invoke();
        }
        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all payments");
            IEnumerable<PlayerPayment> subscriptions = await _paymentDataService.GetAll();
            _payments.Clear();
            _payments.AddRange(subscriptions);
            Loaded?.Invoke();
            SumUpdated?.Invoke();
        }
        public async Task GetAll(DateTime dateFrom, DateTime dateTo)
        {
            _logger.LogInformation(LogFlag + "get all payment from {0} to {1}",dateFrom,dateTo);
            IEnumerable<PlayerPayment> subscriptions = await _paymentDataService.GetAll(dateFrom, dateTo);
            _payments.Clear();
            _payments.AddRange(subscriptions);
            Loaded?.Invoke();
            SumUpdated?.Invoke();
        }
        public Task Initialize()
        {
            throw new NotImplementedException();
        }
        public double GetSum()
        {
            double sum = Payments.Sum(x => x.PaymentValue);
            return sum;
        }
        public async Task Update(PlayerPayment entity)
        {
            _logger.LogInformation(LogFlag + "update payment");
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;

            await _paymentDataService.Update(entity);
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {

                    _logger.LogInformation(LogFlag + "update payment to api");
                    int status = await _paymentApiDataService.Update(entity);
                    if (status == 200)
                    {
                        _logger.LogInformation(LogFlag + "payment synced successfully with code {0}", status.ToString());
                        entity.DataStatus = DataStatus.Synced;
                        await _paymentDataService.UpdateDataStatus(entity);
                    }
                }
                catch(Exception ex) {
                    _logger.LogError(LogFlag + "payment synced failes with error {0}", ex.Message);
                }
            }
            int currentIndex = _payments.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _payments[currentIndex] = entity;
            }
            else
            {
                _payments.Add(entity);
            }
            Updated?.Invoke(entity);
            SumUpdated?.Invoke();
        }

        public async Task SyncPaymentsToCreate()
        {
            IEnumerable<PlayerPayment> payments = await _paymentDataService.GetByDataStatus(DataStatus.ToCreate);
            foreach (PlayerPayment payment in payments)
            {
                _logger.LogInformation(LogFlag + "create payment to api");
                int status = await _paymentApiDataService.Create(payment);
                if (status==201||status==409)
                {
                    _logger.LogInformation(LogFlag + "payment synced successfully with code {0}", status.ToString());
                    payment.DataStatus = DataStatus.Synced;
                    await _paymentDataService.UpdateDataStatus(payment);
                }
                else
                {
                    _logger.LogWarning(LogFlag + "payment synced failed with code {0}", status.ToString());
                }
            }
        }

        public async Task SyncPaymentsToUpdate()
        {
            IEnumerable<PlayerPayment> payments = await _paymentDataService.GetByDataStatus(DataStatus.ToUpdate);
            foreach (PlayerPayment payment in payments)
            {
                _logger.LogInformation(LogFlag + "update payment to api");
                int status = await _paymentApiDataService.Update(payment);
                if (status==200)
                {
                    _logger.LogInformation(LogFlag + "payment synced successfully with code {0}", status.ToString());
                    payment.DataStatus = DataStatus.Synced;
                    await _paymentDataService.UpdateDataStatus(payment);
                }
                else
                {
                    _logger.LogWarning(LogFlag + "payment synced failed with code {0}", status.ToString());
                }
            }
        }
        public async Task SyncPaymentsToDelete()
        {
            IEnumerable<PlayerPayment> payments = await _paymentDataService.GetByDataStatus(DataStatus.ToDelete);
            foreach (PlayerPayment payment in payments)
            {
                _logger.LogInformation(LogFlag + "delete payment from api");
                int status = await _paymentApiDataService.Delete(payment);
                if (status == 200)
                {
                    _logger.LogInformation(LogFlag + "payment synced successfully with code {0}", status.ToString());
                    await _paymentDataService.Delete(payment.Id);
                }
                else
                {
                    _logger.LogWarning(LogFlag + "payment synced failed with code {0}", status.ToString());
                }


            }
        }
    }
}
