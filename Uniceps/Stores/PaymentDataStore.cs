using Uniceps.Core.Models.Employee;
using Uniceps.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.API.Services;
using Uniceps.BackgroundServices;
using Microsoft.Extensions.Logging;
using Uniceps.Stores.ApiDataStores;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Common;

namespace Uniceps.Stores
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
        public PaymentDataStore(IDataService<PlayerPayment> paymentDataService, ILogger<PaymentDataStore> logger, IGetPlayerTransactionService<PlayerPayment> getPlayerTransactionService, IApiDataStore<PlayerPayment> apiDataStore, IDeleteApiDataStore<PlayerPayment> deleteApiDataStore)
        {
            _paymentDataService = paymentDataService;
            _payments = new List<PlayerPayment>();
            _logger = logger;
            _getPlayerTransactionService = getPlayerTransactionService;
            _apiDataStore = apiDataStore;
            _deleteApiDataStore = deleteApiDataStore;
        }

        private readonly IDataService<PlayerPayment> _paymentDataService;
        private readonly IGetPlayerTransactionService<PlayerPayment> _getPlayerTransactionService;
        private readonly List<PlayerPayment> _payments;
        private readonly IApiDataStore<PlayerPayment> _apiDataStore;
        private readonly IDeleteApiDataStore<PlayerPayment> _deleteApiDataStore;
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
            await _apiDataStore.Create(entity);
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
                await _deleteApiDataStore.Delete(entity);
            }
            int currentIndex = _payments.FindIndex(y => y.Id == entity.Id);
            _payments.RemoveAt(currentIndex);
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
            IEnumerable<PlayerPayment> subscriptions = await _getPlayerTransactionService.GetAll(player);
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
            await _apiDataStore.Update(entity);
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

    }
}
