using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;

namespace Uniceps.Stores
{
    public class PeriodReportStore
    {
        private readonly IPeriodReportService _service;
        private readonly List<PlayerPayment> _payments;
        private readonly List<Expenses> _expenses;

        public event Action? PaymentLoaded;
        public event Action? ExpensesLoaded;

        public IEnumerable<PlayerPayment> Payments => _payments;
        public IEnumerable<Expenses> Expenses => _expenses;

        public PeriodReportStore(IPeriodReportService service)
        {
            _service = service;
            _payments = new();
            _expenses = new();
        }
        public async Task GetPayments(DateTime from,DateTime to)
        {
            IEnumerable<PlayerPayment> payments = await _service.GetPayments(from,to);
            _payments.Clear();
            _payments.AddRange(payments);
            PaymentLoaded?.Invoke();
        }
        public async Task GetExpenses(DateTime from, DateTime to)
        {
            IEnumerable<Expenses> expenses = await _service.GetExpenses(from, to);
            _expenses.Clear();
            _expenses.AddRange(expenses);
            ExpensesLoaded?.Invoke();
        }

    }
}
