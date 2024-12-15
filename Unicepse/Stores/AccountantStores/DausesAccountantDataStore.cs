using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Services;

namespace Unicepse.Stores.AccountantStores
{
    public class DausesAccountantDataStore
    {
        string LogFlag = "[DADS] ";
        private readonly ILogger<GymStore> _logger;
        private readonly IEmployeeQuery _employeeDataService;
        private readonly IEmployeeMonthlyTransaction<PlayerPayment> _employeeMonthlyTransaction;
        public event Action? DausesLoaded;

        private readonly List<TrainerDueses> _dauses;

        public DausesAccountantDataStore(ILogger<GymStore> logger, IEmployeeQuery employeeDataService, IEmployeeMonthlyTransaction<PlayerPayment> employeeMonthlyTransaction)
        {
            _logger = logger;
            _employeeDataService = employeeDataService;
            _employeeMonthlyTransaction = employeeMonthlyTransaction;
            _dauses = new List<TrainerDueses>();
        }

        public IEnumerable<TrainerDueses> Dauses => _dauses;
        public async Task GetDauses(DateTime date)
        {
            _logger.LogInformation(LogFlag + "GetDauses");
            _dauses.Clear();
            IEnumerable<Employee> employees = await _employeeDataService.GetAll();

            foreach (Employee employee in employees)
            {
                var trainerDuse = _dauses.Where(x => x.Trainer!.Id == employee.Id && x.IssueDate.Month == date.Month && x.IssueDate.Year == date.Year && x.IssueDate.Day == date.Day).Any();
                if (!trainerDuse)
                {
                    IEnumerable<PlayerPayment> payments = await _employeeMonthlyTransaction.GetAll(employee, date);
                    TrainerDueses MonthlyTrainerDause = new TrainerDueses();
                    MonthlyTrainerDause.TotalSubscriptions = 0;
                    foreach (PlayerPayment pay in payments)
                    {
                        MonthlyTrainerDause.TotalSubscriptions += GetParcent(pay, date);
                    }
                    MonthlyTrainerDause.CountSubscription = payments.GroupBy(x => x.Subscription).Count();
                    MonthlyTrainerDause.Parcent = (double)employee.ParcentValue / 100;
                    MonthlyTrainerDause.IssueDate = date;
                    MonthlyTrainerDause.Trainer = employee;
                    MonthlyTrainerDause.Salary = employee.SalaryValue;
                    MonthlyTrainerDause.CreditsCount =0;
                    _dauses.Add(MonthlyTrainerDause);
                }
            }
            DausesLoaded?.Invoke();
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
                    double total = (days * dayprice);
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
                    double total = (days * dayprice);
                    return total;
                }
            }
            else if (entity.From <= date)
            {
                if (entity.To <= date)
                    return (entity.PaymentValue);
                else if (entity.To > date)
                {
                    int days = (int)date.Subtract(entity.From).TotalDays;
                    double dayprice = entity.PaymentValue / entity.CoverDays;
                    double total = (days * dayprice);
                    return total;
                }
            }


            return 0;
        }
    }
}
