using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.ViewModels.Expenses;
using Unicepse.Stores;

namespace Unicepse.Commands.ExpensesCommands
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
                _expensesListing.ErrorMessage = "خطأ في تحميل المصاريف يرجى اعادة تشغيل البرنامج";
            }
            finally
            {
                _expensesListing.IsLoading = false;
            }
        }
    }
}
