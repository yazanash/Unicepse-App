using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores.AccountantStores;

namespace Unicepse.Stores
{
    public class AccountantDailyStore
    {
        private readonly PaymentsDailyAccountantStore _paymentsDailyAccountantStore;
        private readonly CreditsDailyAccountantStore _creditsDailyAccountantStore;
        private readonly ExpansesDailyAccountantDataStore _expansesDailyAccountantDataStore;
        private readonly SubscriptionDailyAccountantDataStore _subscriptionDailyAccountantDataStore;
        public event Action<double>? DailyPaymentsSumLoaded;
        public event Action<double>? DailySubscriptionsSumLoaded;
        public event Action<double>? DailyExpensesSumLoaded;
        public event Action<double>? DailyCreditsSumLoaded;
        public AccountantDailyStore(PaymentsDailyAccountantStore paymentsDailyAccountantStore, CreditsDailyAccountantStore creditsDailyAccountantStore, ExpansesDailyAccountantDataStore expansesDailyAccountantDataStore, SubscriptionDailyAccountantDataStore subscriptionDailyAccountantDataStore)
        {
            _paymentsDailyAccountantStore = paymentsDailyAccountantStore;
            _creditsDailyAccountantStore = creditsDailyAccountantStore;
            _expansesDailyAccountantDataStore = expansesDailyAccountantDataStore;
            _subscriptionDailyAccountantDataStore = subscriptionDailyAccountantDataStore;
        }
        public double GetPaymentSum()
        {
            //_logger.LogInformation(LogFlag + "GetPaymentSum");
            double sum = _paymentsDailyAccountantStore.Payments.Sum(x => x.PaymentValue);
            return sum;
        }
        public double GetSubscriptionsSum()
        {
            //_logger.LogInformation(LogFlag + "GetPaymentSum");
            double sum =_subscriptionDailyAccountantDataStore.Subscriptions.Count();
            return sum;
        }
        public double GetExpensesSum()
        {
            //_logger.LogInformation(LogFlag + "GetExpensesSum");
            double sum =_expansesDailyAccountantDataStore.Expenses.Sum(x => x.Value);
            return sum;
        }
        public double GetEmployeeCreditsSum()
        {
            //_logger.LogInformation(LogFlag + "GetEmployeeCreditsSum");
            double sum = _creditsDailyAccountantStore.EmployeeCredits.Sum(x => x.CreditValue);
            return sum;
        }
        public async Task GetStates(DateTime date)
        {
            //_logger.LogInformation(LogFlag + "GetStates");
            await _paymentsDailyAccountantStore.GetDaily(date);
            double paySum = GetPaymentSum();
            await _subscriptionDailyAccountantDataStore.GetDaily(date);
            double subsSum = GetSubscriptionsSum();
            await _creditsDailyAccountantStore.GetDaily(date);
            double creditSum = GetEmployeeCreditsSum();
            await _expansesDailyAccountantDataStore.GetDaily(date);
            double expSum = GetExpensesSum();
            DailySubscriptionsSumLoaded?.Invoke(subsSum);
            DailyPaymentsSumLoaded?.Invoke(paySum);
            DailyExpensesSumLoaded?.Invoke(expSum);
            DailyCreditsSumLoaded?.Invoke(creditSum);
            //_payments.Clear();
            //_employeeCredits.Clear();
            //_expenses.Clear();
        }
    }
}
