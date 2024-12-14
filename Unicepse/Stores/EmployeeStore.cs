using Unicepse.Core.Models.Employee;
using Unicepse.Entityframework.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;
using Unicepse.Core.Models.Sport;
using Microsoft.Extensions.Logging;
using Unicepse.Core.Services;

namespace Unicepse.Stores
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
        public event Action<Filter?>? FilterChanged;
        public event Action<Sport?>? SportChanged;
        private readonly ILogger<EmployeeStore> _logger;
        private readonly IDataService<Employee> _employeeDataService;
        private readonly IDeleteConnectionService<Employee> _deleteConnectionService;
        private readonly List<Employee> _employee;
        private readonly Lazy<Task> _initializeLazy;
        public IEnumerable<Employee> Employees => _employee;
        private readonly List<Sport> _sports;
        public IEnumerable<Sport> Sports => _sports;

        public EmployeeStore(IDataService<Employee> employeeDataService, ILogger<EmployeeStore> logger, IDeleteConnectionService<Employee> deleteConnectionService)
        {
            _employeeDataService = employeeDataService;
            _employee = new List<Employee>();
            _sports = new List<Sport>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _logger = logger;
            _deleteConnectionService = deleteConnectionService;
        }


        private Employee? _selectedEmployee;
        public Employee? SelectedEmployee
        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                _selectedEmployee = value;
                _sports.Clear();
                if (SelectedEmployee != null)
                {
                    _sports.Add(new Sport() { Id = -1, Name = "الكل" });
                    _sports.AddRange(SelectedEmployee.Sports!);

                }
                StateChanged?.Invoke(SelectedEmployee);
            }
        }


        private Sport? _selectedSport;
        public Sport? SelectedSport
        {
            get
            {
                return _selectedSport;
            }
            set
            {
                _selectedSport = value;
               
            SportChanged?.Invoke(SelectedSport);
            }
        }

        public event Action<Employee?>? StateChanged;

        private Filter? _selectedFilter;
        public Filter? SelectedFilter
        {
            get
            {
                return _selectedFilter;
            }
            set
            {
                _selectedFilter = value;
                FilterChanged?.Invoke(_selectedFilter);
            }
        }

        public async Task Add(Employee entity)
        {
            _logger.LogInformation(LogFlag + "Add employee");
            await _employeeDataService.Create(entity);
            _employee.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(Employee employee)
        {
            _logger.LogInformation(LogFlag+ "delete employee");
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
        public async Task DeleteConnectedSports(int Id)
        {
            _logger.LogInformation(LogFlag + "delete trainer sports connection started");
            await _deleteConnectionService.DeleteConnection(Id);

        }
    }
}
