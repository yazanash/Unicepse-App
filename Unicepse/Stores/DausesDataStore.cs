using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Subscription;
using Unicepse.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Stores
{
    public class DausesDataStore : IDataStore<TrainerDueses>
    {
        private readonly PaymentDataService _paymentDataService;
        private readonly DausesDataService _dausesDataService;
        private readonly EmployeeDataService _employeeDataService;
        private readonly EmployeeCreditsDataService _employeeCreditsDataService;
        public List<TrainerDueses> _dauses;
        public List<PlayerPayment> _payments;
        public IEnumerable<TrainerDueses> Dauses => _dauses;
        public TrainerDueses? MonthlyTrainerDause { get; set; }
        public event Action<TrainerDueses?>? StateChanged;
        public event Action<TrainerDueses>? Created;
        public event Action? Loaded;
        public event Action<TrainerDueses>? Updated;
        public event Action<int>? Deleted;
        private readonly Lazy<Task> _initializeLazy;
        public DausesDataStore(PaymentDataService paymentDataService, DausesDataService dausesDataService, EmployeeCreditsDataService employeeCreditsDataService, EmployeeDataService employeeDataService)
        {
            _paymentDataService = paymentDataService;
            _dausesDataService = dausesDataService;
            _dauses = new List<TrainerDueses>();
            _payments = new List<PlayerPayment>();
            _employeeCreditsDataService = employeeCreditsDataService;
            _employeeDataService = employeeDataService;

            _initializeLazy = new Lazy<Task>(Initialize);
        }
        public async Task GetMonthlyReport(Employee trainer, DateTime date)
        {
            IEnumerable<PlayerPayment> payments = await _paymentDataService.GetTrainerPayments(trainer, date);
            IEnumerable<Credit> Credits = await _employeeCreditsDataService.GetAll(trainer, date);
            MonthlyTrainerDause = new TrainerDueses();
            MonthlyTrainerDause.TotalSubscriptions = 0;
            foreach (PlayerPayment pay in payments)
            {
                MonthlyTrainerDause.TotalSubscriptions += _dausesDataService.GetParcent(pay, date);
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
            StateChanged?.Invoke(MonthlyTrainerDause);
        }

        public async Task GetAll()
        {
            IEnumerable<TrainerDueses> employees = await _dausesDataService.GetAll();
            _dauses.Clear();
            _dauses.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task GetAll(Employee employee)
        {
            IEnumerable<TrainerDueses> employees = await _dausesDataService.GetAll(employee);
            _dauses.Clear();
            _dauses.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task GetAll(Employee employee, DateTime date)
        {
            IEnumerable<TrainerDueses> employees = await _dausesDataService.GetAll(employee, date);
            _dauses.Clear();
            _dauses.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task Add(TrainerDueses entity)
        {
            await _dausesDataService.Create(entity);
            _dauses.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Update(TrainerDueses entity)
        {
            await _dausesDataService.Update(entity);
            int currentIndex = _dauses.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _dauses[currentIndex] = entity;
            }
            else
            {
                _dauses.Add(entity);
            }
            Updated?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            bool deleted = await _dausesDataService.Delete(entity_id);
            int currentIndex = _dauses.FindIndex(y => y.Id == entity_id);
            _dauses.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task Initialize()
        {
            IEnumerable<Employee> employees = await _employeeDataService.GetAllParcentTrainers();
            DateTime date = DateTime.Now;
            foreach (Employee employee in employees)
            {
                var trainerDuse = _dauses.Where(x => x.IssueDate.Month == date.Month && x.IssueDate.Year == date.Year && x.IssueDate.Day == date.Day).Any();
                if (!trainerDuse)
                {
                    IEnumerable<PlayerPayment> payments = await _paymentDataService.GetTrainerPayments(employee, date);
                    IEnumerable<Credit> Credits = await _employeeCreditsDataService.GetAll(employee, date);
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
                    if (MonthlyTrainerDause.CreditsCount > 0)
                        MonthlyTrainerDause.Credits = Credits.Sum(x => x.CreditValue);
                    else
                        MonthlyTrainerDause.Credits = 0;
                    StateChanged?.Invoke(MonthlyTrainerDause);
                    var td = _dauses.Where(x => x.IssueDate.Month == MonthlyTrainerDause.IssueDate.Month && x.IssueDate.Year == MonthlyTrainerDause.IssueDate.Year).SingleOrDefault();
                    if (td != null)
                    {
                        if (MonthlyTrainerDause.IssueDate > td.IssueDate)
                        {
                            MonthlyTrainerDause.Id = td.Id;
                            await Update(MonthlyTrainerDause);
                        }
                    }
                    else
                        await Add(MonthlyTrainerDause);
                }
            }

        }
    }
}
