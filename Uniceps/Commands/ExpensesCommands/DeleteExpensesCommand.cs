using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores;

namespace Uniceps.Commands.ExpensesCommands
{
    public class DeleteExpensesCommand : AsyncCommandBase
    {
        private readonly ExpensesDataStore _expensesDataStore;

        public DeleteExpensesCommand(ExpensesDataStore expensesDataStore)
        {
            _expensesDataStore = expensesDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _expensesDataStore.Delete(_expensesDataStore.SelectedExpenses!.Id);
        }
    }
}
