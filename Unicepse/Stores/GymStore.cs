using Unicepse.Core.Models;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Expenses;
using Unicepse.Core.Models.Payment;
using Unicepse.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Stores
{
    public class GymStore
    {
        private readonly EmployeeDataService _employeeDataService;
        private readonly EmployeeCreditsDataService _employeeCreditsDataService;
        private readonly ExpensesDataService _expensesDataService;

        private readonly DausesDataService _dausesDataService;

        #region Payment
        public event Action? PaymentsLoaded;
        public event Action<double>? DailyPaymentsSumLoaded;

        private readonly PaymentDataService _paymentDataService;
        private readonly List<PlayerPayment> _payments;
        public IEnumerable<PlayerPayment> Payments => _payments;

        public async Task GetAllPayments(DateTime dateFrom, DateTime dateTo)
        {
            IEnumerable<PlayerPayment> subscriptions = await _paymentDataService.GetAll(dateFrom, dateTo);
            _payments.Clear();
            _payments.AddRange(subscriptions);
            PaymentsLoaded?.Invoke();
        }
        public async Task GetDailyPayments(DateTime dateTo)
        {
            IEnumerable<PlayerPayment> subscriptions = await _paymentDataService.GetAll(dateTo);
            _payments.Clear();
            _payments.AddRange(subscriptions);
            PaymentsLoaded?.Invoke();
        }
        public async Task GetLastMonthPayments(DateTime dateFrom, DateTime dateTo)
        {
            IEnumerable<PlayerPayment> subscriptions = await _paymentDataService.GetAll(dateFrom, dateTo);
            _payments.Clear();
            _payments.AddRange(subscriptions);
            PaymentsLoaded?.Invoke();
        }
        #endregion

        #region  Expenses
        public event Action? ExpensesLoaded;
        public event Action<double>? DailyExpensesSumLoaded;
        private readonly List<Expenses> _expenses;
        public IEnumerable<Expenses> Expenses => _expenses;
        public async Task GetPeriodExpenses(DateTime dateFrom, DateTime dateTo)
        {
            IEnumerable<Expenses> expenses = await _expensesDataService.GetPeriodExpenses(dateFrom, dateTo);
            _expenses.Clear();
            _expenses.AddRange(expenses);
            ExpensesLoaded?.Invoke();
        }
        public async Task GetDailyExpenses(DateTime dateTo)
        {
            IEnumerable<Expenses> expenses = await _expensesDataService.GetPeriodExpenses(dateTo);
            _expenses.Clear();
            _expenses.AddRange(expenses);
            ExpensesLoaded?.Invoke();
        }
        #endregion

        #region Dauses
        public event Action? DausesLoaded;

        private readonly List<TrainerDueses> _dauses;
        public IEnumerable<TrainerDueses> Dauses => _dauses;

        public async Task GetDauses(DateTime date)
        {
            IEnumerable<Employee> employees = await _employeeDataService.GetAllParcentTrainers();

            foreach (Employee employee in employees)
            {
                var trainerDuse = _dauses.Where(x => x.Trainer!.Id == employee.Id && x.IssueDate.Month == date.Month && x.IssueDate.Year == date.Year && x.IssueDate.Day == date.Day).Any();
                if (!trainerDuse)
                {
                    IEnumerable<PlayerPayment> payments = await _paymentDataService.GetTrainerPayments(employee, date);
                    TrainerDueses MonthlyTrainerDause = new TrainerDueses();
                    MonthlyTrainerDause.TotalSubscriptions = 0;
                    foreach (PlayerPayment pay in payments)
                    {
                        MonthlyTrainerDause.TotalSubscriptions += _dausesDataService.GetParcent(pay, date);
                    }
                    MonthlyTrainerDause.CountSubscription = payments.GroupBy(x => x.Subscription).Count();
                    MonthlyTrainerDause.Parcent = (double)employee.ParcentValue / 100;
                    MonthlyTrainerDause.IssueDate = date;
                    MonthlyTrainerDause.Trainer = employee;
                    MonthlyTrainerDause.Salary = employee.SalaryValue;
                    MonthlyTrainerDause.CreditsCount = Credits.Count();
                    _dauses.Add(MonthlyTrainerDause);
                }
            }
            DausesLoaded?.Invoke();
        }

        #endregion

        #region Credits
        public event Action? CreditsLoaded;
        public event Action<double>? DailyCreditsSumLoaded;

        private readonly List<Employee> _credits;
        public IEnumerable<Employee> Credits => _credits;
        public async Task GetAllCredits()
        {
            IEnumerable<Employee> employees = await _employeeDataService.GetAll();
            _credits.Clear();
            _credits.AddRange(employees);
            CreditsLoaded?.Invoke();
        }

        private readonly List<Credit> _employeeCredits;
        public IEnumerable<Credit> EmployeeCredits => _employeeCredits;
        public async Task GetDailyCredits(DateTime date)
        {
            IEnumerable<Credit> credits = await _employeeCreditsDataService.GetAll(date);
            _employeeCredits.Clear();
            _employeeCredits.AddRange(credits);
        }
        #endregion

        public event Action<MonthlyReport>? ReportLoaded;

        public double GetPaymentSum()
        {
            double sum = Payments.Sum(x => x.PaymentValue);
            return sum;
        }
        public double GetThisMouthPaymentSum(DateTime date)
        {
            double sum = 0;
            foreach (var pay in _payments)
            {
                sum += _dausesDataService.GetParcent(pay, date);
            }
            return sum;
        }
        public double GetExpensesSum()
        {
            double sum = Expenses.Sum(x => x.Value);
            return sum;
        }
        public double GetDausesSum()
        {
            double sum = Dauses.Sum(x => x.TotalSubscriptions * x.Parcent);
            return sum;
        }
        public double GetCreditsSum()
        {
            double sum = Credits.Sum(x => x.SalaryValue);
            return sum;
        }
        public double GetEmployeeCreditsSum()
        {
            double sum = _employeeCredits.Sum(x => x.CreditValue);
            return sum;
        }
        public GymStore(EmployeeDataService employeeDataService, ExpensesDataService expensesDataService, PaymentDataService paymentDataService, DausesDataService dausesDataService, EmployeeCreditsDataService employeeCreditsDataService)
        {
            _employeeDataService = employeeDataService;
            _expensesDataService = expensesDataService;
            _paymentDataService = paymentDataService;
            _dausesDataService = dausesDataService;
            _payments = new List<PlayerPayment>();
            _expenses = new List<Expenses>();
            _credits = new List<Employee>();
            _employeeCredits = new List<Credit>();
            _dauses = new List<TrainerDueses>();
            _employeeCreditsDataService = employeeCreditsDataService;
        }

        public async Task GetReport(DateTime date)
        {
            DateTime firstDayInMonth = new DateTime(date.Year, date.Month, 1);
            DateTime LastDayInMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            MonthlyReport monthlyReport = new();
            await GetAllPayments(firstDayInMonth, LastDayInMonth);
            monthlyReport.TotalIncome = GetPaymentSum();
            monthlyReport.IncomeForNextMonth = monthlyReport.TotalIncome - GetThisMouthPaymentSum(LastDayInMonth);
            DateTime firstDayInLastMonth = firstDayInMonth.AddMonths(-1);
            DateTime LastDayInLastMonth = new DateTime(firstDayInLastMonth.Year, firstDayInLastMonth.Month, DateTime.DaysInMonth(firstDayInLastMonth.Year, firstDayInLastMonth.Month));
            await GetAllPayments(firstDayInLastMonth, LastDayInLastMonth);
            monthlyReport.IncomeFromLastMonth = GetPaymentSum() - GetThisMouthPaymentSum(LastDayInLastMonth);
            await GetPeriodExpenses(firstDayInMonth, LastDayInMonth);
            monthlyReport.Expenses = GetExpensesSum();

            await GetDauses(LastDayInMonth);
            monthlyReport.TrainerDauses = GetDausesSum();
            await GetAllCredits();
            monthlyReport.Salaries = GetCreditsSum();
            monthlyReport.NetIncome = monthlyReport.TotalIncome - monthlyReport.IncomeForNextMonth + monthlyReport.IncomeFromLastMonth - monthlyReport.TrainerDauses - monthlyReport.Salaries - monthlyReport.Expenses;

            ReportLoaded?.Invoke(monthlyReport);
            _dauses.Clear();
            _expenses.Clear();
            _credits.Clear();
            _payments.Clear();
        }


        public async Task GetStates(DateTime date)
        {

            await GetDailyPayments(date);
            double paySum = GetPaymentSum();
            await GetDailyCredits(date);
            double creditSum = GetEmployeeCreditsSum();
            await GetDailyExpenses(date);
            double expSum = GetExpensesSum();
            DailyPaymentsSumLoaded?.Invoke(paySum);
            DailyExpensesSumLoaded?.Invoke(expSum);
            DailyCreditsSumLoaded?.Invoke(creditSum);
            _payments.Clear();
            _employeeCredits.Clear();
            _expenses.Clear();
        }


    }
}
