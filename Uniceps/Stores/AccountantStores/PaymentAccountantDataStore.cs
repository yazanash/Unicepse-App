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
    public class PaymentAccountantDataStore : IAccountantStore<PlayerPayment>
    {
        private readonly IPeriodReportService<PlayerPayment> _paymentsPeriodReportService;
        private readonly ILogger<IAccountantStore<PlayerPayment>> _logger;
        private readonly List<PlayerPayment> _payments;
        public IEnumerable<PlayerPayment> Payments => _payments;
        public event Action? PaymentsLoaded;
        string LogFlag = "[PADS] ";
        public PaymentAccountantDataStore(IPeriodReportService<PlayerPayment> paymentsPeriodReportService, ILogger<IAccountantStore<PlayerPayment>> logger)
        {
            _paymentsPeriodReportService = paymentsPeriodReportService;
            _logger = logger;
            _payments = new List<PlayerPayment>();
        }


        public async Task GetAll(DateTime dateFrom, DateTime dateTo)
        {
            _logger.LogInformation(LogFlag + "Get all payment");
            IEnumerable<PlayerPayment> subscriptions = await _paymentsPeriodReportService.GetAll(dateFrom, dateTo);
            _payments.Clear();
            _payments.AddRange(subscriptions);
            PaymentsLoaded?.Invoke();
        }
    }
}
