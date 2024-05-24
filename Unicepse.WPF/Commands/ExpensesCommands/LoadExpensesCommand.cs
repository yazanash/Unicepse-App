using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.WPF.Commands.ExpensesCommands
{
    public class LoadExpensesCommand : AsyncCommandBase
    {

        private readonly ExpensesDataStore _expensesStore;
        private readonly ExpensesListViewModel _expensesListing;

        public LoadExpensesCommand(ExpensesDataStore expensesStore, ExpensesListViewModel expensesListing)
        {
            _expensesStore = expensesStore;
            _expensesListing = expensesListing;
        }
        public async override Task ExecuteAsync(object? parameter)
        {
            _expensesListing.ErrorMessage = null;
            _expensesListing.IsLoading = true;

            try
            {
                await _expensesStore.GetAll();
            }
            catch (Exception)
            {
                _expensesListing.ErrorMessage = "Failed to load Expenses. Please restart the application.";
            }
            finally
            {
                _expensesListing.IsLoading = false;
            }
        }
    }
}
