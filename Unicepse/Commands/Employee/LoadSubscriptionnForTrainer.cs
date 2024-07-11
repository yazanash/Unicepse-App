﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Commands;
using Unicepse.Stores;

namespace Unicepse.Commands.Employee
{
    public class LoadSubscriptionnForTrainer : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;

        public LoadSubscriptionnForTrainer(EmployeeStore employeeStore, SubscriptionDataStore subscriptionDataStore)
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
