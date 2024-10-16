using Unicepse.Core.Models.Employee;
using Unicepse.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Unicepse.Stores
{
    public class CreditsDataStore : IDataStore<Credit>
    {
        string LogFlag = "[Credits] ";
        private readonly EmployeeCreditsDataService _employeeCreditsDataService;
        private readonly ILogger<CreditsDataStore> _logger;
        public List<Credit> _credits;
        public IEnumerable<Credit> Credits => _credits;
        public CreditsDataStore(EmployeeCreditsDataService employeeCreditsDataService, ILogger<CreditsDataStore> logger)
        {
            _employeeCreditsDataService = employeeCreditsDataService;
            _credits = new List<Credit>();
            _logger = logger;
        }

        public event Action<Credit>? Created;
        public event Action? Loaded;
        public event Action<Credit>? Updated;
        public event Action<int>? Deleted;

        private Credit? _selectedCredit;
        public Credit? SelectedCredit
        {
            get
            {
                return _selectedCredit;
            }
            set
            {
                _selectedCredit = value;
                StateChanged?.Invoke(SelectedCredit);
            }
        }
        public event Action<Credit?>? StateChanged;
        public async Task Add(Credit entity)
        {
            _logger.LogInformation(LogFlag+"Add credit started");
            await _employeeCreditsDataService.Create(entity);
            _credits.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            _logger.LogInformation(LogFlag + "delete credit started");
            bool deleted = await _employeeCreditsDataService.Delete(entity_id);
            int currentIndex = _credits.FindIndex(y => y.Id == entity_id);
            _credits.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all started");
            IEnumerable<Credit> employees = await _employeeCreditsDataService.GetAll();
            _credits.Clear();
            _credits.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task GetAll(Employee employee)
        {
            _logger.LogInformation(LogFlag + "get all employee credit started");
            IEnumerable<Credit> employees = await _employeeCreditsDataService.GetAll(employee);
            _credits.Clear();
            _credits.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task GetAll(Employee employee, DateTime date)
        {
            _logger.LogInformation(LogFlag + "get all employee credit by date started");
            IEnumerable<Credit> employees = await _employeeCreditsDataService.GetAll(employee, date);
            _credits.Clear();
            _credits.AddRange(employees);
            Loaded?.Invoke();
        }
        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public async Task Update(Credit entity)
        {
            _logger.LogInformation(LogFlag + "update credit started");
            await _employeeCreditsDataService.Update(entity);
            int currentIndex = _credits.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _credits[currentIndex] = entity;
            }
            else
            {
                _credits.Add(entity);
            }
            Updated?.Invoke(entity);
        }
    }
}
