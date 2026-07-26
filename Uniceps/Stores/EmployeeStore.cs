using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services;
using Uniceps.MessengerSystem;
using Uniceps.MessengerSystem.Events;
using Uniceps.utlis.common;

namespace Uniceps.Stores
{
    public enum EmployeeRole
    {
        Trainer,
        Secretaria,
        Employee,
    }
    public class EmployeeStore : IDataStore<Employee>
    {
        string LogFlag = "[Employee] ";
        public event Action<Employee>? Created;
        public event Action? Loaded;
        public event Action<Employee>? Updated;
        public event Action<int>? Deleted;
        private readonly ILogger<EmployeeStore> _logger;
        private readonly IDataService<Employee> _employeeDataService;
        private readonly List<Employee> _employee;
        private readonly Lazy<Task> _initializeLazy;
        private readonly LicenseStore _licenseStore;
        public IEnumerable<Employee> Employees => _employee;
        private readonly List<Sport> _sports;
        public IEnumerable<Sport> Sports => _sports;


        public EmployeeStore(IDataService<Employee> employeeDataService, ILogger<EmployeeStore> logger, LicenseStore licenseStore)
        {
            _employeeDataService = employeeDataService;
            _employee = new List<Employee>();
            _sports = new List<Sport>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _logger = logger;
            _licenseStore = licenseStore;
        }

        public async Task Add(Employee entity)
        {
            int empcount = _employee.Where(x => !x.IsTrainer).Count();
            int trcount = _employee.Where(x => x.IsTrainer).Count();
            if (!_licenseStore.Current.IsFullVersion && (empcount >= 2 || trcount >= 2))
                throw new FreeLimitException("لقد وصلت الحد الاعلى من النسخة المجانية ... اشترك الان لتحصل عدد غير محدود");
            _logger.LogInformation(LogFlag + "Add employee");
            Employee creetedEntity = await _employeeDataService.Create(entity);
            _employee.Add(creetedEntity);
            Created?.Invoke(creetedEntity);
        }

        public async Task Delete(Employee employee)
        {
            _logger.LogInformation(LogFlag + "delete employee");
            employee.IsActive = false;
            await _employeeDataService.Update(employee);
            int currentIndex = _employee.FindIndex(y => y.Id == employee.Id);
            _employee.RemoveAt(currentIndex);
            Deleted?.Invoke(employee.Id);
        }
        public async Task Delete(int id)
        {
            _logger.LogInformation(LogFlag + "force delete employee started");
            await _employeeDataService.Delete(id);
            int currentIndex = _employee.FindIndex(y => y.Id == id);
            _employee.RemoveAt(currentIndex);
            Deleted?.Invoke(id);
        }
        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all employee started");
            IEnumerable<Employee> employees = await _employeeDataService.GetAll();
            _employee.Clear();
            _employee.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task Initialize()
        {
            _logger.LogInformation(LogFlag + "get all employee init started");
            IEnumerable<Employee> employees = await _employeeDataService.GetAll();
            _employee.Clear();
            _employee.AddRange(employees);
        }

        public async Task Update(Employee entity)
        {
            _logger.LogInformation(LogFlag + "update employee started");
            await _employeeDataService.Update(entity);
            int currentIndex = _employee.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _employee[currentIndex] = entity;
            }
            else
            {
                _employee.Add(entity);
            }
            if (entity.IsTrainer)
                Messenger.Default.Send(new EntityUpdated<Employee>(entity));
            Updated?.Invoke(entity);
        }

        public async Task FilterEmployee(EmployeeRole employeeRole)
        {
            _logger.LogInformation(LogFlag + "filter employees");
            IEnumerable<Employee> employees = await _employeeDataService.GetAll();
            _employee.Clear();
            switch (employeeRole)
            {
                case EmployeeRole.Trainer:
                    IEnumerable<Employee> Trainers = employees.Where(x => x.IsTrainer);
                    _employee.AddRange(Trainers);
                    Loaded?.Invoke();
                    break;
                case EmployeeRole.Secretaria:
                    IEnumerable<Employee> Secrtaria = employees.Where(x => x.IsSecrtaria);
                    _employee.AddRange(Secrtaria);
                    Loaded?.Invoke();
                    break;
                case EmployeeRole.Employee:
                    IEnumerable<Employee> Employee = employees.Where(x => !x.IsTrainer && !x.IsSecrtaria);
                    _employee.AddRange(Employee);
                    Loaded?.Invoke();
                    break;
            }

        }
    }
}
