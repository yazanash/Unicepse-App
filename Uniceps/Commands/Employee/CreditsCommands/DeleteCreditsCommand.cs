using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores;

namespace Uniceps.Commands.Employee.CreditsCommands
{
    public class DeleteCreditsCommand : AsyncCommandBase
    {
        private readonly CreditsDataStore _creditsDataStore;

        public DeleteCreditsCommand(CreditsDataStore creditsDataStore)
        {
            _creditsDataStore = creditsDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _creditsDataStore.Delete(_creditsDataStore.SelectedCredit!.Id);
        }
    }
}
