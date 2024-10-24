﻿using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Subscription;
using Unicepse.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
        string LogFlag = "[Dauses] ";
        private readonly ILogger<DausesDataStore> _logger;
        public IEnumerable<TrainerDueses> Dauses => _dauses;

        public IEnumerable<PlayerPayment> Payments => _payments;

        public TrainerDueses? MonthlyTrainerDause { get; set; }
        public event Action<TrainerDueses?>? StateChanged;
        public event Action<TrainerDueses?>? ReportLoaded;
        public event Action<TrainerDueses>? Created;
        public event Action? Loaded;
        public event Action<TrainerDueses>? Updated;
        public event Action<int>? Deleted;
        //private readonly Lazy<Task> _initializeLazy;
        public DausesDataStore(PaymentDataService paymentDataService, DausesDataService dausesDataService, EmployeeCreditsDataService employeeCreditsDataService, EmployeeDataService employeeDataService, ILogger<DausesDataStore> logger)
        {
            _paymentDataService = paymentDataService;
            _dausesDataService = dausesDataService;
            _dauses = new List<TrainerDueses>();
            _payments = new List<PlayerPayment>();
            _employeeCreditsDataService = employeeCreditsDataService;
            _employeeDataService = employeeDataService;
            _logger = logger;

            //_initializeLazy = new Lazy<Task>(Initialize);
        }
        public async Task GetMonthlyReport(Employee trainer, DateTime date)
        {
            _logger.LogInformation(LogFlag+"Get trainer payments");
            IEnumerable<PlayerPayment> payments = await _paymentDataService.GetTrainerPayments(trainer, date);
            _logger.LogInformation(LogFlag + "Get trainer credits");
            IEnumerable<Credit> Credits = await _employeeCreditsDataService.GetAll(trainer, date);
            _payments.Clear();
            _payments.AddRange(payments);
            _logger.LogInformation(LogFlag + "create trainer dueses");
            MonthlyTrainerDause = new TrainerDueses();
            MonthlyTrainerDause.TotalSubscriptions = 0;
            foreach (PlayerPayment pay in payments)
            {
                MonthlyTrainerDause.TotalSubscriptions += _dausesDataService.GetParcent(pay, date);
            }
            List<Subscription> subs = new List<Subscription>();
            foreach(var item in payments.GroupBy(x => x.Subscription))
            {
                if (item.Key != null&&!subs.Any(x => x.Id == item.Key.Id))
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
            IEnumerable<PlayerPayment> payments = await _paymentDataService.GetTrainerPayments(trainer, date);
            _logger.LogInformation(LogFlag + "Get trainer credits");
            IEnumerable<Credit> Credits = await _employeeCreditsDataService.GetAll(trainer, date);
            _payments.Clear();
            _payments.AddRange(payments);
            _logger.LogInformation(LogFlag + "create trainer dueses");
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
            ReportLoaded?.Invoke(MonthlyTrainerDause);
        }


        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all dueses");
            IEnumerable<TrainerDueses> employees = await _dausesDataService.GetAll();
            _dauses.Clear();
            _dauses.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task GetAll(Employee employee)
        {
            _logger.LogInformation(LogFlag + "get all trainer dueses");
            IEnumerable<TrainerDueses> employees = await _dausesDataService.GetAll(employee);
            _dauses.Clear();
            _dauses.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task GetAll(Employee employee, DateTime date)
        {
            _logger.LogInformation(LogFlag + "get all trainer dueses by date : " + date);
            IEnumerable<TrainerDueses> employees = await _dausesDataService.GetAll(employee, date);
            _dauses.Clear();
            _dauses.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task Add(TrainerDueses entity)
        {
            _logger.LogInformation(LogFlag + "add trainer dueses by date : ");
            await _dausesDataService.Create(entity);
            _dauses.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Update(TrainerDueses entity)
        {
            _logger.LogInformation(LogFlag + "update trainer dueses");
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
            _logger.LogInformation(LogFlag + "delete trainer dueses");
            bool deleted = await _dausesDataService.Delete(entity_id);
            int currentIndex = _dauses.FindIndex(y => y.Id == entity_id);
            _dauses.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task Initialize()
        {
            _logger.LogInformation(LogFlag + "initialize trainer dueses");
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
