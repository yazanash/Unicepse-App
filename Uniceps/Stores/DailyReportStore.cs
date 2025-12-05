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
using Uniceps.Entityframework.Services;

namespace Uniceps.Stores
{
    public class DailyReportStore
    {
        private readonly IDailyReportService _service;
        public event Action? PaymentLoaded;
        public event Action? CreditsLoaded;
        public event Action? SubscriptionsLoaded;
        public event Action? ExpensesLoaded;

        private readonly List<PlayerPayment> _payments;
        private readonly List<Credit> _credits;
        private readonly List<Subscription> _subscriptions;
        private readonly List<Expenses> _expenses;

        public event Action<double>? PaymentSumLoaded;
        public event Action<double>? CreditsSumLoaded;
        public event Action<double>? SubscriptionsSumLoaded;
        public event Action<double>? ExpensesSumLoaded;

        public IEnumerable<PlayerPayment> Payments => _payments;
        public IEnumerable<Credit> Credits => _credits;
        public IEnumerable<Subscription> Subscriptions => _subscriptions;
        public IEnumerable<Expenses> Expenses => _expenses;
        public DailyReportStore(IDailyReportService service)
        {
            _service = service;
            _payments = new();
            _credits = new();
            _subscriptions = new();
            _expenses = new();
        }
        public async Task GetPayments(DateTime date)
        {
            IEnumerable<PlayerPayment> payments = await _service.GetPayments(date);
            _payments.Clear();
            _payments.AddRange(payments);
            PaymentLoaded?.Invoke();
        }
        public async Task GetCredits(DateTime date)
        {
            IEnumerable<Credit> credits = await _service.GetCredits(date);
            _credits.Clear();
            _credits.AddRange(credits);
            CreditsLoaded?.Invoke();
        }
        public async Task GetSubscriptions(DateTime date)
        {
            IEnumerable<Subscription> subscriptions = await _service.GetSubscriptions(date);
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            SubscriptionsLoaded?.Invoke();
        }
        public async Task GetExpenses(DateTime date)
        {
            IEnumerable<Expenses> expenses = await _service.GetExpenses(date);
            _expenses.Clear();
            _expenses.AddRange(expenses);
            ExpensesLoaded?.Invoke();
        }

        internal async Task GetStates(DateTime date)
        {
            IEnumerable<PlayerPayment> payments = await _service.GetPayments(date);
            IEnumerable<Credit> credits = await _service.GetCredits(date);
            IEnumerable<Subscription> subscriptions = await _service.GetSubscriptions(date);
            IEnumerable<Expenses> expenses = await _service.GetExpenses(date);
            double paymentSum = payments.Sum(x => x.PaymentValue);
            double subscriptionsSum = subscriptions.Sum(x => x.PriceAfterOffer);
            double creditsSum = credits.Sum(x => x.CreditValue);
            double expensesSum = expenses.Sum(x => x.Value);
            PaymentSumLoaded?.Invoke(paymentSum);
            CreditsSumLoaded?.Invoke(creditsSum);
            SubscriptionsSumLoaded?.Invoke(subscriptionsSum);
            ExpensesSumLoaded?.Invoke(expensesSum);
        }
    }
}
