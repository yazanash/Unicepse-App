using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Expenses;
using Unicepse.Core.Services;

namespace Unicepse.Stores.AccountantStores
{
    public class ExpansesDailyAccountantDataStore : IDailyAccountantStore<Expenses>
    {
        private readonly IDailyTransactionService<Expenses> _expensesDailyTransactionService;
        private readonly ILogger<IDailyAccountantStore<Expenses>> _logger;
        private readonly List<Expenses> _expenses;
        public IEnumerable<Expenses> Expenses => _expenses;
        public event Action? ExpensesLoaded;
        string LogFlag = "[EDADS] ";

        public ExpansesDailyAccountantDataStore(IDailyTransactionService<Expenses> expensesDailyTransactionService, ILogger<IDailyAccountantStore<Expenses>> logger)
        {
            _expensesDailyTransactionService = expensesDailyTransactionService;
            _logger = logger;
            _expenses = new List<Expenses>();
        }

        public async Task GetDaily(DateTime dateTo)
        {
            _logger.LogInformation(LogFlag + "GetDailyExpenses");
            IEnumerable<Expenses> expenses = await _expensesDailyTransactionService.GetAll(dateTo);
            _expenses.Clear();
            _expenses.AddRange(expenses);
            ExpensesLoaded?.Invoke();
        }

    }
}
