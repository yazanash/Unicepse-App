using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Expenses;
using Uniceps.Entityframework.Services;

namespace Uniceps.Stores
{
    public class ExpensesDataStore : IDataStore<Expenses>
    {

        public event Action<Expenses>? Created;
        public event Action? Loaded;
        public event Action<Expenses>? Updated;
        public event Action<int>? Deleted;
        string LogFlag = "[Expenses] ";
        private readonly ILogger<EmployeeStore> _logger;

        private readonly ExpensesDataService _expensesDataService;
        private readonly List<Expenses> _expenses;
        private readonly Lazy<Task> _initializeLazy;
        public IEnumerable<Expenses> Expenses => _expenses;

        public ExpensesDataStore(ExpensesDataService expensesDataService, ILogger<EmployeeStore> logger)
        {
            _expensesDataService = expensesDataService;
            _expenses = new List<Expenses>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _logger = logger;
        }

        private Expenses? _selectedExpenses;
        public Expenses? SelectedExpenses
        {
            get
            {
                return _selectedExpenses;
            }
            set
            {
                _selectedExpenses = value;
                _logger.LogInformation(LogFlag + "selected expenses changed");
                StateChanged?.Invoke(SelectedExpenses);
            }
        }
        public event Action<Expenses?>? StateChanged;
        public async Task Add(Expenses entity)
        {
            _logger.LogInformation(LogFlag + "add expenses");
            await _expensesDataService.Create(entity);
            _expenses.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            _logger.LogInformation(LogFlag + "delete expenses");
            bool deleted = await _expensesDataService.Delete(entity_id);
            int currentIndex = _expenses.FindIndex(y => y.Id == entity_id);
            _expenses.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all expenses from init function");
            await _initializeLazy.Value;
            Loaded?.Invoke();
        }
        public async Task Initialize()
        {
            _logger.LogInformation(LogFlag + "init expenses");
            IEnumerable<Expenses> expenses = await _expensesDataService.GetAll();
            _expenses.Clear();
            _expenses.AddRange(expenses);
        }

        public async Task Update(Expenses entity)
        {
            _logger.LogInformation(LogFlag + "update expenses");
            await _expensesDataService.Update(entity);
            int currentIndex = _expenses.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _expenses[currentIndex] = entity;
            }
            else
            {
                _expenses.Add(entity);
            }
            Updated?.Invoke(entity);
        }
        public double GetSum()
        {
            _logger.LogInformation(LogFlag + "get expenses sum");
            double sum = Expenses.Sum(x => x.Value);
            return sum;
        }
    }
}
