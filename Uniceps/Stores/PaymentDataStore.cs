using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.BackgroundServices;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services;
using Uniceps.MessengerSystem;
using Uniceps.MessengerSystem.Events;
using Uniceps.Stores.ApiDataStores;

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
        public PaymentDataStore(IDataService<PlayerPayment> paymentDataService, ILogger<PaymentDataStore> logger, IGetPlayerTransactionService<PlayerPayment> getPlayerTransactionService)
        {
            _paymentDataService = paymentDataService;
            _payments = new List<PlayerPayment>();
            _logger = logger;
            _getPlayerTransactionService = getPlayerTransactionService;
        }

        private readonly IDataService<PlayerPayment> _paymentDataService;
        private readonly IGetPlayerTransactionService<PlayerPayment> _getPlayerTransactionService;
        private readonly List<PlayerPayment> _payments;
        public IEnumerable<PlayerPayment> Payments => _payments;

        public async Task Add(PlayerPayment entity)
        {
            _logger.LogInformation(LogFlag + "add payment");
            await _paymentDataService.Create(entity);
            _payments.Add(entity);
            Messenger.Default.Send(new PaymentCreated(
            SubscriptionId: entity.SubscriptionId,
            AmountPaid: entity.PaymentValue,
            DatePaid: entity.PayDate
        ));
            Created?.Invoke(entity);
            SumUpdated?.Invoke();
        }
        public async Task Delete(int entity_id)
        {
            _logger.LogInformation(LogFlag + "force delete payment");
            bool deleted = await _paymentDataService.Delete(entity_id);
          
            int currentIndex = _payments.FindIndex(y => y.Id == entity_id);
            PlayerPayment entity = _payments[currentIndex];
            Messenger.Default.Send(new PaymentDeleted(
           SubscriptionId: entity.SubscriptionId,
           AmountPaid: entity.PaymentValue
       ));
            _payments.RemoveAt(currentIndex);

            Deleted?.Invoke(entity_id);
            SumUpdated?.Invoke();

        }
        public async Task GetAllByPlayer(int playerId)
        {
            _logger.LogInformation(LogFlag + "get player payments");
            IEnumerable<PlayerPayment> subscriptions = await _getPlayerTransactionService.GetAllByPlayer(playerId);
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

            await _paymentDataService.Update(entity);
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
