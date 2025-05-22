using Uniceps.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Stores
{
    public class DausesDataStore
    {
        private readonly IEmployeeMonthlyTransaction<Credit> _employeeMonthlyTransaction;
        private readonly IEmployeeMonthlyTransaction<PlayerPayment> _paymentEmployeeMonthlyTransaction;
        public List<PlayerPayment> _payments;
        string LogFlag = "[Dauses] ";
        private readonly ILogger<DausesDataStore> _logger;
        public IEnumerable<PlayerPayment> Payments => _payments;

        public TrainerDueses? MonthlyTrainerDause { get; set; }
        public event Action<TrainerDueses?>? StateChanged;
        public event Action<TrainerDueses?>? ReportLoaded;
        public DausesDataStore(ILogger<DausesDataStore> logger, IEmployeeMonthlyTransaction<Credit> employeeMonthlyTransaction, IEmployeeMonthlyTransaction<PlayerPayment> paymentEmployeeMonthlyTransaction)
        {
            _payments = new List<PlayerPayment>();
            _logger = logger;
            _employeeMonthlyTransaction = employeeMonthlyTransaction;
            _paymentEmployeeMonthlyTransaction = paymentEmployeeMonthlyTransaction;
        }
        public async Task GetMonthlyReport(Employee trainer, DateTime date)
        {
            _logger.LogInformation(LogFlag + "Get trainer payments");
            IEnumerable<PlayerPayment> payments = await _paymentEmployeeMonthlyTransaction.GetAll(trainer, date);
            _logger.LogInformation(LogFlag + "Get trainer credits");
            IEnumerable<Credit> Credits = await _employeeMonthlyTransaction.GetAll(trainer, date);
            _payments.Clear();
            _payments.AddRange(payments);
            _logger.LogInformation(LogFlag + "create trainer dueses");
            MonthlyTrainerDause = new TrainerDueses();
            MonthlyTrainerDause.TotalSubscriptions = 0;
            foreach (PlayerPayment pay in payments)
            {
                MonthlyTrainerDause.TotalSubscriptions += GetParcent(pay, date);
            }
            List<Subscription> subs = new List<Subscription>();
            foreach (var item in payments.GroupBy(x => x.Subscription))
            {
                if (item.Key != null && !subs.Any(x => x.Id == item.Key.Id))
                {
                    MonthlyTrainerDause.CountSubscription++;
                    subs.Add(item.Key);
                }
            }

            MonthlyTrainerDause.Parcent = (double)trainer.ParcentValue / 100;
            MonthlyTrainerDause.IssueDate = date;
            MonthlyTrainerDause.Trainer = trainer;
            MonthlyTrainerDause.Salary = trainer.SalaryValue;
            MonthlyTrainerDause.CreditsCount = Credits.Count();
            if (MonthlyTrainerDause.CreditsCount > 0)
                MonthlyTrainerDause.Credits = Credits.Sum(x => x.CreditValue);
            else
                MonthlyTrainerDause.Credits = 0;
            StateChanged?.Invoke(MonthlyTrainerDause);
        }
        public async Task GetPrintedMonthlyReport(Employee trainer, DateTime date)
        {
            _logger.LogInformation(LogFlag + "Get trainer payments");
            IEnumerable<PlayerPayment> payments = await _paymentEmployeeMonthlyTransaction.GetAll(trainer, date);
            _logger.LogInformation(LogFlag + "Get trainer credits");
            IEnumerable<Credit> Credits = await _employeeMonthlyTransaction.GetAll(trainer, date);
            _payments.Clear();
            _payments.AddRange(payments);
            _logger.LogInformation(LogFlag + "create trainer dueses");
            MonthlyTrainerDause = new TrainerDueses();
            MonthlyTrainerDause.TotalSubscriptions = 0;
            foreach (PlayerPayment pay in payments)
            {
                MonthlyTrainerDause.TotalSubscriptions += GetParcent(pay, date);
            }
            MonthlyTrainerDause.CountSubscription = payments.GroupBy(x => x.Subscription).Count();
            MonthlyTrainerDause.Parcent = (double)trainer.ParcentValue / 100;
            MonthlyTrainerDause.IssueDate = date;
            MonthlyTrainerDause.Trainer = trainer;
            MonthlyTrainerDause.Salary = trainer.SalaryValue;
            MonthlyTrainerDause.CreditsCount = Credits.Count();
            if (MonthlyTrainerDause.CreditsCount > 0)
                MonthlyTrainerDause.Credits = Credits.Sum(x => x.CreditValue);
            else
                MonthlyTrainerDause.Credits = 0;
            ReportLoaded?.Invoke(MonthlyTrainerDause);
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
