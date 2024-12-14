using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Services;

namespace Unicepse.Stores.EmployeeStores
{
    public class EmployeeSubscriptionDataStore
    {
        private readonly IEmployeeMonthlyTransaction<Subscription> _employeeMonthlyTransaction;
        private readonly ILogger<EmployeeSubscriptionDataStore> _logger;
        private readonly List<Subscription> _subscriptions;
        public IEnumerable<Subscription> Subscriptions => _subscriptions;
        string LogFlag = "[Subscriptions] ";
        public event Action? Loaded;
        public EmployeeSubscriptionDataStore(IEmployeeMonthlyTransaction<Subscription> employeeMonthlyTransaction, ILogger<EmployeeSubscriptionDataStore> logger)
        {
            _employeeMonthlyTransaction = employeeMonthlyTransaction;
            _logger = logger;
            _subscriptions = new List<Subscription>();
        }

        public async Task GetAll(Employee trainer, DateTime date)
        {
            _logger.LogInformation(LogFlag + "get all subscription for trainer in date {0}", date);
            IEnumerable<Subscription> subscriptions = await _employeeMonthlyTransaction.GetAll(trainer, date);
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            Loaded?.Invoke();
        }

    }
}
