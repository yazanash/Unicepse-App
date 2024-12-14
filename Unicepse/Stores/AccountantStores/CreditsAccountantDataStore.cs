using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Services;

namespace Unicepse.Stores.AccountantStores
{
    public class CreditsAccountantDataStore 
    {
        private readonly IEmployeeQuery _employeeDataService;
        string LogFlag = "[CADS] ";
        private readonly ILogger<GymStore> _logger;

        public CreditsAccountantDataStore(IEmployeeQuery employeeDataService, ILogger<GymStore> logger)
        {
            _employeeDataService = employeeDataService;
            _logger = logger;
            _credits = new List<Employee>();
        }

        private readonly List<Employee> _credits;
        public event Action? CreditsLoaded;
        public IEnumerable<Employee> Credits => _credits;
        public async Task GetAllCredits()
        {
            _logger.LogInformation(LogFlag + "GetAllCredits");
            IEnumerable<Employee> employees = await _employeeDataService.GetAll();
            _credits.Clear();
            _credits.AddRange(employees);
            CreditsLoaded?.Invoke();
        }
    }
}
