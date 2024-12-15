using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Payment;
using Unicepse.Stores;
using Unicepse.Stores.AccountantStores;
using Unicepse.ViewModels.Accountant;

namespace Unicepse.Commands.AccountantCommand
{
    public class LoadDailyPayments : AsyncCommandBase
    {
        private readonly IDailyAccountantStore<PlayerPayment> _gymStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public LoadDailyPayments(IDailyAccountantStore<PlayerPayment> gymStore, AccountingStateViewModel accountingStateViewModel)
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
