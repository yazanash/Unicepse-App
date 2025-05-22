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
        private readonly NavigationService<ExpensesListViewModel> _navigationService;
        private EditExpenseViewModel _editExpenseViewModel;

        public EditExpensesCommand(ExpensesDataStore expensesDataStore, NavigationService<ExpensesListViewModel> navigationService, EditExpenseViewModel editExpenseViewModel)
        {
            _expensesDataStore = expensesDataStore;
            _navigationService = navigationService;
            _editExpenseViewModel = editExpenseViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            Expenses expenses = new Expenses()
            {
                Id = _expensesDataStore.SelectedExpenses!.Id,
                Description = _editExpenseViewModel.Descriptiones,
                date = _editExpenseViewModel.ExpensesDate,
                Value = _editExpenseViewModel.ExpensesValue,

            };
            await _expensesDataStore.Update(expenses);
            _navigationService.Navigate();
        }
    }
}
