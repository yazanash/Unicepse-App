using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores.AccountantStores;
using Uniceps.ViewModels.Accountant;
using Uniceps.Stores;

namespace Uniceps.Commands.AccountantCommand
{
    public class LoadDailyExpenses : AsyncCommandBase
    {
        private readonly ExpansesDailyAccountantDataStore _gymStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public LoadDailyExpenses(ExpansesDailyAccountantDataStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            _gymStore = gymStore;
            _accountingStateViewModel = accountingStateViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _gymStore.GetDaily(_accountingStateViewModel.Date);
        }
    }
}
