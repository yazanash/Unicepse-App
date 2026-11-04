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
        public LoadDailyPayments(DailyReportStore dailyReportStore)
        {
            _dailyReportStore = dailyReportStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is DateTime date)
            await _dailyReportStore.GetPayments(date);
        }
    }
}
