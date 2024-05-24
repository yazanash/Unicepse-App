using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class CreditsDataStore : IDataStore<Credit>
    {
        private readonly EmployeeCreditsDataService _employeeCreditsDataService;
        public List<Credit> _credits;
        public IEnumerable<Credit> Credits => _credits;
        public CreditsDataStore(EmployeeCreditsDataService employeeCreditsDataService)
        {
            _employeeCreditsDataService = employeeCreditsDataService;
            _credits = new List<Credit>();
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
            await _employeeCreditsDataService.Create(entity);
            _credits.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            bool deleted = await _employeeCreditsDataService.Delete(entity_id);
            int currentIndex = _credits.FindIndex(y => y.Id == entity_id);
            _credits.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            IEnumerable<Credit> employees = await _employeeCreditsDataService.GetAll();
            _credits.Clear();
            _credits.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task GetAll(Employee employee)
        {
            IEnumerable<Credit> employees = await _employeeCreditsDataService.GetAll(employee);
            _credits.Clear();
            _credits.AddRange(employees);
            Loaded?.Invoke();
        }
        public async Task GetAll(Employee employee,DateTime date)
        {
            IEnumerable<Credit> employees = await _employeeCreditsDataService.GetAll(employee,date);
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
