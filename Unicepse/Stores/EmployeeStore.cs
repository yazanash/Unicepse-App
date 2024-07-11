﻿using Unicepse.Core.Models.Employee;
using Unicepse.Entityframework.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;

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
        public event Action<Employee>? Created;
        public event Action? Loaded;
        public event Action<Employee>? Updated;
        public event Action<int>? Deleted;
        public event Action<Filter?>? FilterChanged;

        private readonly EmployeeDataService _employeeDataService;
        private readonly DausesDataService _dausesDataService;
        private readonly List<Employee> _employee;
        private readonly Lazy<Task> _initializeLazy;
        public IEnumerable<Employee> Employees => _employee;
        public EmployeeStore(EmployeeDataService employeeDataService, DausesDataService dausesDataService)
        {
            _employeeDataService = employeeDataService;
            _employee = new List<Employee>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _dausesDataService = dausesDataService;
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
                StateChanged?.Invoke(SelectedEmployee);
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
            await _employeeDataService.Create(entity);
            _employee.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(Employee employee)
        {
            employee.IsActive = false;
            await _employeeDataService.Update(employee);
            int currentIndex = _employee.FindIndex(y => y.Id == employee.Id);
            _employee.RemoveAt(currentIndex);
            Deleted?.Invoke(employee.Id);
        }
        public async Task Delete(int id)
        {
            await _employeeDataService.Delete(id);
            int currentIndex = _employee.FindIndex(y => y.Id == id);
            _employee.RemoveAt(currentIndex);
            Deleted?.Invoke(id);
        }
        public async Task GetAll()
        {
            await _initializeLazy.Value;
            Loaded?.Invoke();
        }
        public async Task Initialize()
        {
            IEnumerable<Employee> employees = await _employeeDataService.GetAll();
            _employee.Clear();
            _employee.AddRange(employees);
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
            await _employeeDataService.DeleteConnectedSports(Id);

        }
    }
}
