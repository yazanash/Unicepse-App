using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores;

namespace Uniceps.Commands.Employee.CreditsCommands
{
    public class LoadCreditsCommand : AsyncCommandBase
    {
        private readonly CreditsDataStore _creditDataStore;

        public LoadCreditsCommand(CreditsDataStore creditDataStore)
        {
            _creditDataStore = creditDataStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            if(parameter is int employeeId)
            await _creditDataStore.GetAll(employeeId);
        }
    }
}
