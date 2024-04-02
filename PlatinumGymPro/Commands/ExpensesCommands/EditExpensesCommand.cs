using PlatinumGym.Core.Models.Expenses;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.ExpensesCommands
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
