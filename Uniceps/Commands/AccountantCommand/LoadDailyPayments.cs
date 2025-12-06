using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Core.Models.Payment;
using Uniceps.ViewModels.Accountant;
using Uniceps.Stores;

namespace Uniceps.Commands.AccountantCommand
{
    public class LoadDailyPayments : AsyncCommandBase
    {
        private readonly DailyReportStore _dailyReportStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public LoadDailyPayments(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            _dailyReportStore = dailyReportStore;
            _accountingStateViewModel = accountingStateViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _dailyReportStore.GetPayments(_accountingStateViewModel.Date);
        }
    }
}
