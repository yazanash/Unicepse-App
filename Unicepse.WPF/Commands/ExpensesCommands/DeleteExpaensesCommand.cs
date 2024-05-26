using Unicepse.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.WPF.Commands.ExpensesCommands
{
    public class DeleteExpaensesCommand : AsyncCommandBase
    {
        private readonly ExpensesDataStore _expensesDataStore;

        public DeleteExpaensesCommand(ExpensesDataStore expensesDataStore)
        {
            _expensesDataStore = expensesDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _expensesDataStore.Delete(_expensesDataStore.SelectedExpenses!.Id);
        }
    }
}
