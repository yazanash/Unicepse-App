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
    public class SubmitExpensesCommand : AsyncCommandBase
    {
        private readonly ExpensesDataStore _expensesDataStore;
        private readonly NavigationService<ExpensesListViewModel> _navigationService;
        private AddExpenseViewModel _addExpenseViewModel;

        public SubmitExpensesCommand(ExpensesDataStore expensesDataStore, NavigationService<ExpensesListViewModel> navigationService, AddExpenseViewModel addExpenseViewModel)
        {
            _expensesDataStore = expensesDataStore;
            _navigationService = navigationService;
            _addExpenseViewModel = addExpenseViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            Expenses expenses = new Expenses()
            {
                Description = _addExpenseViewModel.Descriptiones,
                date = _addExpenseViewModel.ExpensesDate,
                Value = _addExpenseViewModel.ExpensesValue,

            };
            await _expensesDataStore.Add(expenses);
            _navigationService.Navigate();
        }
    }
}
