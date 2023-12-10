using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Accountant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.AccountantCommand
{
    public class LoadStatesCommand : AsyncCommandBase
    {
        private readonly AccountingStateViewModel _accountingStateViewModel;
        private readonly ExpensesDataStore _expensesDataStore;

        public LoadStatesCommand(AccountingStateViewModel accountingStateViewModel, ExpensesDataStore expensesDataStore)
        {
            _accountingStateViewModel = accountingStateViewModel;
            _expensesDataStore = expensesDataStore;
        }

        public  override async Task ExecuteAsync(object? parameter)
        {
            await _expensesDataStore.GetAll(); 
        }
    }
}
