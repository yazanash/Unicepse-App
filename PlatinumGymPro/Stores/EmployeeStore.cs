using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Entityframework.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public enum EmployeeRole
    {
        Trainer,
        Secretaria,
        Employee,
    }
    public class EmployeeStore : IDataStore<Employee>
    {
        public event Action<Employee>? Created;
        public event Action<IEnumerable<Employee>>? Loaded;
        public event Action<Employee>? Updated;
        public event Action<bool>? Deleted;


        private readonly EmployeeDataService _employeeDataService;
        private readonly List<Employee> _employee;
        private readonly Lazy<Task> _initializeLazy;
        public IEnumerable<Employee> Sports => _employee;
        public EmployeeStore(EmployeeDataService employeeDataService, List<Employee> employee)
        {
            _employeeDataService = employeeDataService;
            _employee = employee;
            _initializeLazy = new Lazy<Task>(Initialize);
        }

        public async Task Add(Employee entity)
        {
            await _employeeDataService.Create(entity);
            _employee.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            bool deleted = await _employeeDataService.Delete(entity_id);
            int currentIndex = _employee.FindIndex(y => y.Id == entity_id);
            _employee.RemoveAt(currentIndex);
            Deleted?.Invoke(deleted);
        }

        public async Task GetAll()
        {
          await _initializeLazy.Value;
        }
        public async Task Initialize()
        {
            IEnumerable<Employee> employees = await _employeeDataService.GetAll();
            _employee.Clear();
            _employee.AddRange(employees);
            Loaded?.Invoke(employees);
        }

        public async Task Update(Employee entity)
        {
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
            IEnumerable<Employee> employees = await _employeeDataService.GetAll();
            _employee.Clear();
            switch (employeeRole)
            {
                case EmployeeRole.Trainer:
                    IEnumerable<Employee> Trainers = employees.Where(x => x.IsTrainer);
                    _employee.AddRange(Trainers);
                    Loaded?.Invoke(Trainers);
                    break;
                case EmployeeRole.Secretaria:
                    IEnumerable<Employee> Secrtaria = employees.Where(x => x.IsSecrtaria);
                    _employee.AddRange(Secrtaria);
                    Loaded?.Invoke(Secrtaria);
                    break;
                case EmployeeRole.Employee:
                    IEnumerable<Employee> Employee = employees.Where(x => !x.IsTrainer && !x.IsSecrtaria);
                    _employee.AddRange(Employee);
                    Loaded?.Invoke(Employee);
                    break;
            }
            
        }
    }
}
