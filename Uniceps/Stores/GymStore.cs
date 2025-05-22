using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Uniceps.Core.Services;
using Uniceps.Stores.AccountantStores;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models;

namespace Uniceps.Stores
{
    public class GymStore
    {
        private readonly PaymentAccountantDataStore _paymentAccountantDataStore;
        private readonly ExpensesAccountantDataStore _expensesAccountantDataStore;
        private readonly CreditsAccountantDataStore _creditsAccountantDataStore;
        private readonly DausesAccountantDataStore _dausesAccountantDataStore;
        string LogFlag = "[Gym] ";
        private readonly ILogger<GymStore> _logger;
        public event Action<MonthlyReport>? ReportLoaded;
        public GymStore(ILogger<GymStore> logger, PaymentAccountantDataStore paymentAccountantDataStore, ExpensesAccountantDataStore expensesAccountantDataStore, CreditsAccountantDataStore creditsAccountantDataStore, DausesAccountantDataStore dausesAccountantDataStore)
        {
            _logger = logger;
            _paymentAccountantDataStore = paymentAccountantDataStore;
            _expensesAccountantDataStore = expensesAccountantDataStore;
            _creditsAccountantDataStore = creditsAccountantDataStore;
            _dausesAccountantDataStore = dausesAccountantDataStore;
        }

        public async Task GetReport(DateTime date)
        {
            _logger.LogInformation(LogFlag + "GetReport");
            DateTime firstDayInMonth = new DateTime(date.Year, date.Month, 1);
            DateTime LastDayInMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            MonthlyReport monthlyReport = new();
            _logger.LogInformation(LogFlag + "Get payments from: {0} to: {1}", firstDayInMonth, LastDayInMonth);
            await _paymentAccountantDataStore.GetAll(firstDayInMonth, LastDayInMonth);

            monthlyReport.TotalIncome = GetPaymentSum();
            monthlyReport.IncomeForNextMonth = monthlyReport.TotalIncome - GetThisMouthPaymentSum(LastDayInMonth);
            DateTime firstDayInLastMonth = firstDayInMonth.AddMonths(-1);
            DateTime LastDayInLastMonth = new DateTime(firstDayInLastMonth.Year, firstDayInLastMonth.Month, DateTime.DaysInMonth(firstDayInLastMonth.Year, firstDayInLastMonth.Month));
            _logger.LogInformation(LogFlag + "Get payments from: {0} to: {1}", firstDayInLastMonth, LastDayInLastMonth);
            await _paymentAccountantDataStore.GetAll(firstDayInLastMonth, LastDayInLastMonth);
            monthlyReport.IncomeFromLastMonth = GetPaymentSum() - GetThisMouthPaymentSum(LastDayInLastMonth);
            _logger.LogInformation(LogFlag + "Get expenses from: {0} to: {1}", firstDayInMonth, LastDayInMonth);
            await _expensesAccountantDataStore.GetAll(firstDayInMonth, LastDayInMonth);
            monthlyReport.Expenses = GetExpensesSum();
            _logger.LogInformation(LogFlag + "Get dauses to: {0}", LastDayInMonth);
            await _dausesAccountantDataStore.GetDauses(LastDayInMonth);
            monthlyReport.TrainerDauses = GetDausesSum();
            _logger.LogInformation(LogFlag + "Get all credits");
            await _creditsAccountantDataStore.GetAllCredits();
            monthlyReport.Salaries = GetCreditsSum();
            monthlyReport.NetIncome = monthlyReport.TotalIncome - monthlyReport.IncomeForNextMonth + monthlyReport.IncomeFromLastMonth - monthlyReport.TrainerDauses - monthlyReport.Salaries - monthlyReport.Expenses;

            ReportLoaded?.Invoke(monthlyReport);
            //_dauses.Clear();
            //_expenses.Clear();
            //_credits.Clear();
            //_payments.Clear();
        }

        public double GetPaymentSum()
        {
            _logger.LogInformation(LogFlag + "GetPaymentSum");
            double sum = _paymentAccountantDataStore.Payments.Sum(x => x.PaymentValue);
            return sum;
        }
        public double GetThisMouthPaymentSum(DateTime date)
        {
            _logger.LogInformation(LogFlag + "GetThisMouthPaymentSum");
            double sum = 0;
            foreach (var pay in _paymentAccountantDataStore.Payments)
            {
                sum += GetParcent(pay, date);
            }
            return sum;
        }
        public double GetExpensesSum()
        {
            _logger.LogInformation(LogFlag + "GetExpensesSum");
            double sum = _expensesAccountantDataStore.Expenses.Sum(x => x.Value);
            return sum;
        }
        public double GetDausesSum()
        {
            _logger.LogInformation(LogFlag + "GetDausesSum");
            double sum = _dausesAccountantDataStore.Dauses.Sum(x => x.TotalSubscriptions * x.Parcent);
            return sum;
        }
        public double GetCreditsSum()
        {
            _logger.LogInformation(LogFlag + "GetCreditsSum");
            double sum = _creditsAccountantDataStore.Credits.Sum(x => x.SalaryValue);
            return sum;
        }

        public double GetParcent(PlayerPayment entity, DateTime date)
        {
            if (entity.PayDate.Month == date.AddMonths(-1).Month)
            {
                DateTime firstDayInMonth = new DateTime(date.Year, date.Month, 1);

                if (entity.To <= date)
                {
                    int days = 0;
                    if (entity.From > firstDayInMonth)
                        days = (int)entity.To.Subtract(entity.From).TotalDays + 1;
                    else
                        days = (int)entity.To.Subtract(firstDayInMonth).TotalDays + 1;
                    double dayprice = entity.PaymentValue / entity.CoverDays;
                    double total = days * dayprice;
                    return total;
                }
                else if (entity.To > date)
                {
                    int days = 0;
                    if (entity.From > firstDayInMonth)
                        days = (int)date.Subtract(entity.From).TotalDays;
                    else
                        days = (int)date.Subtract(firstDayInMonth).TotalDays + 1;
                    double dayprice = entity.PaymentValue / entity.CoverDays;
                    double total = days * dayprice;
                    return total;
                }
            }
            else if (entity.From <= date)
            {
                if (entity.To <= date)
                    return entity.PaymentValue;
                else if (entity.To > date)
                {
                    int days = (int)date.Subtract(entity.From).TotalDays;
                    double dayprice = entity.PaymentValue / entity.CoverDays;
                    double total = days * dayprice;
                    return total;
                }
            }


            return 0;
        }
    }
}
