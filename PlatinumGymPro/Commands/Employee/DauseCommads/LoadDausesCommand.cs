using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.Employee.DauseCommads
{
    public class LoadDausesCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;

        public LoadDausesCommand(EmployeeStore employeeStore, DausesDataStore dausesDataStore)
        {
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            await _dausesDataStore.GetAll(_employeeStore.SelectedEmployee!);
        }
    }
}
