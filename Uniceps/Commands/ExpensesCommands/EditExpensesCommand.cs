using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Expenses;
using Uniceps.Core.Models.Expenses;

namespace Uniceps.Commands.ExpensesCommands
{
    public class EditExpensesCommand : AsyncCommandBase
    {
        private readonly ExpensesDataStore _expensesDataStore;
        private EditExpenseViewModel _editExpenseViewModel;

        public EditExpensesCommand(ExpensesDataStore expensesDataStore,  EditExpenseViewModel editExpenseViewModel)
        {
            _expensesDataStore = expensesDataStore;
            _editExpenseViewModel = editExpenseViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            Expenses expenses = new Expenses()
            {
                Id = _editExpenseViewModel.SelectedExpensesListItemViewModel.Expenses!.Id,
                Description = _editExpenseViewModel.Descriptiones,
                date = _editExpenseViewModel.ExpensesDate,
                Value = _editExpenseViewModel.ExpensesValue,

            };
            await _expensesDataStore.Update(expenses);
            _editExpenseViewModel.OnExpensesUpdated();
        }
    }
}
