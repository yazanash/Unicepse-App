using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.ViewModels.Accountant;
using Uniceps.Stores;

namespace Uniceps.Commands.AccountantCommand
{
    public class LoadDailyCredits : AsyncCommandBase
    {
        private readonly DailyReportStore _dailyReportStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public LoadDailyCredits(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            _dailyReportStore = dailyReportStore;
            _accountingStateViewModel = accountingStateViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _dailyReportStore.GetCredits(_accountingStateViewModel.Date);
        }
    }
}
