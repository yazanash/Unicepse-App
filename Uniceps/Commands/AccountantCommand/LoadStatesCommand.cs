using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores;
using Uniceps.ViewModels.Accountant;

namespace Uniceps.Commands.AccountantCommand
{
    public class LoadStatesCommand : AsyncCommandBase
    {
        private readonly AccountingStateViewModel _accountingStateViewModel;
        private readonly DailyReportStore _dailyReportStore;
        public LoadStatesCommand(AccountingStateViewModel accountingStateViewModel, DailyReportStore dailyReportStore)
        {
            _accountingStateViewModel = accountingStateViewModel;
            _dailyReportStore = dailyReportStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _dailyReportStore.GetStates(_accountingStateViewModel.Date);
        }
    }
}
