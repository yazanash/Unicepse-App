using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;

namespace Uniceps.Stores.AccountantStores
{
    public class PaymentsDailyAccountantStore : IDailyAccountantStore<PlayerPayment>
    {
        private readonly IDailyTransactionService<PlayerPayment> _paymentsDailyTransactionService;
        private readonly ILogger<IDailyAccountantStore<PlayerPayment>> _logger;
        private readonly List<PlayerPayment> _payments;
        public IEnumerable<PlayerPayment> Payments => _payments;
        public event Action? PaymentsLoaded;
        string LogFlag = "[PDADS] ";

        public PaymentsDailyAccountantStore(IDailyTransactionService<PlayerPayment> paymentsDailyTransactionService, ILogger<IDailyAccountantStore<PlayerPayment>> logger)
        {
            _paymentsDailyTransactionService = paymentsDailyTransactionService;
            _logger = logger;
            _payments = new List<PlayerPayment>();
        }

        public async Task GetDaily(DateTime dateTo)
        {
            _logger.LogInformation(LogFlag + "Get daily payment");
            IEnumerable<PlayerPayment> subscriptions = await _paymentsDailyTransactionService.GetAll(dateTo);
            _payments.Clear();
            _payments.AddRange(subscriptions);
            PaymentsLoaded?.Invoke();
        }
    }
}
