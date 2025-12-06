using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores;
using Uniceps.Stores.EmployeeStores;

namespace Uniceps.Commands.Employee
{
    public class LoadSubscriptionnForTrainer : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly EmployeeSubscriptionDataStore _subscriptionDataStore;

        public LoadSubscriptionnForTrainer(EmployeeStore employeeStore, EmployeeSubscriptionDataStore subscriptionDataStore)
        {
            _employeeStore = employeeStore;
            _subscriptionDataStore = subscriptionDataStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            await _subscriptionDataStore.GetAll(_employeeStore.SelectedEmployee!, DateTime.Now);
        }
    }
}
