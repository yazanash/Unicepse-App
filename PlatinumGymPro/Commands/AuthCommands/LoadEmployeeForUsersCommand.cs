using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.AuthCommands
{
    public class LoadEmployeeForUsersCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;

        public LoadEmployeeForUsersCommand(EmployeeStore employeeStore)
        {
            this._employeeStore = employeeStore;
           
        }

        public override async Task ExecuteAsync(object? parameter)
        {
                await _employeeStore.GetAll();
        }
    }
}
