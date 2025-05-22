using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Services;
using Uniceps.Stores;

namespace Uniceps.Stores.AccountantStores
{
    public class CreditsDailyAccountantStore : IDailyAccountantStore<Credit>
    {
        private readonly IDailyTransactionService<Credit> _creditsDailyTransactionService;
        private readonly ILogger<GymStore> _logger;
        string LogFlag = "[CDADS] ";
        private readonly List<Credit> _employeeCredits;
        public IEnumerable<Credit> EmployeeCredits => _employeeCredits;
        public event Action? EmployeeCreditsLoaded;
        public CreditsDailyAccountantStore(IDailyTransactionService<Credit> creditsDailyTransactionService, ILogger<GymStore> logger)
        {
            _creditsDailyTransactionService = creditsDailyTransactionService;
            _logger = logger;
            _employeeCredits = new List<Credit>();
        }

        public async Task GetDaily(DateTime dateTo)
        {
            _logger.LogInformation(LogFlag + "GetDailyCredits");
            IEnumerable<Credit> credits = await _creditsDailyTransactionService.GetAll(dateTo);
            _employeeCredits.Clear();
            _employeeCredits.AddRange(credits);
            EmployeeCreditsLoaded?.Invoke();
        }
    }
}
