using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.Employee.CreditsCommands
{
    public class LoadCreditsCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditDataStore;

        public LoadCreditsCommand(EmployeeStore employeeStore, CreditsDataStore creditDataStore)
        {
            _employeeStore = employeeStore;
            _creditDataStore = creditDataStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            await _creditDataStore.GetAll(_employeeStore.SelectedEmployee!);
        }
    }
}
