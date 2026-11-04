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
        public LoadDailyCredits(DailyReportStore dailyReportStore)
        {
            _dailyReportStore = dailyReportStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is DateTime date) 
            await _dailyReportStore.GetCredits(date);
        }
    }
}
