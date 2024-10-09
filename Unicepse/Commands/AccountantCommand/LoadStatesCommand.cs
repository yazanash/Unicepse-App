using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Commands;
using Unicepse.Stores;
using Unicepse.ViewModels.Accountant;

namespace Unicepse.Commands.AccountantCommand
{
    public class LoadStatesCommand : AsyncCommandBase
    {
        private readonly AccountingStateViewModel _accountingStateViewModel;
        private readonly ExpensesDataStore _expensesDataStore;
        private readonly GymStore _gymStore;
        public LoadStatesCommand(AccountingStateViewModel accountingStateViewModel, ExpensesDataStore expensesDataStore, GymStore gymStore)
        {
            _accountingStateViewModel = accountingStateViewModel;
            _expensesDataStore = expensesDataStore;
            _gymStore = gymStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _gymStore.GetStates(DateTime.Now);
        }
    }
}
