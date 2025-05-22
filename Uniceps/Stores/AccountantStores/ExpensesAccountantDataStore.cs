using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Payment;

namespace Uniceps.Stores.AccountantStores
{
    public class ExpensesAccountantDataStore : IAccountantStore<Expenses>
    {
        private readonly IPeriodReportService<Expenses> _expensesPeriodReportService;
        private readonly ILogger<IAccountantStore<Expenses>> _logger;
        private readonly List<Expenses> _expenses;
        public IEnumerable<Expenses> Expenses => _expenses;
        public event Action? ExpensesLoaded;
        string LogFlag = "[EADS] ";

        public ExpensesAccountantDataStore(IPeriodReportService<Expenses> expensesPeriodReportService, ILogger<IAccountantStore<Expenses>> logger)
        {
            _expensesPeriodReportService = expensesPeriodReportService;
            _logger = logger;
            _expenses = new List<Expenses>();
        }

        public async Task GetAll(DateTime dateFrom, DateTime dateTo)
        {
            _logger.LogInformation(LogFlag + "GetPeriodExpenses");
            IEnumerable<Expenses> expenses = await _expensesPeriodReportService.GetAll(dateFrom, dateTo);
            _expenses.Clear();
            _expenses.AddRange(expenses);
            ExpensesLoaded?.Invoke();
        }



    }
}
