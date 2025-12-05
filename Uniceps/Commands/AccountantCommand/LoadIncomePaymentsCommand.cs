using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Stores;
using Uniceps.Commands;
using Uniceps.ViewModels.Accountant;

namespace Uniceps.Commands.AccountantCommand
{
    public class LoadIncomePaymentsCommand : AsyncCommandBase
    {
        private readonly PeriodReportStore _periodReportStore;
        private readonly IncomeReportViewModel _incomeReportViewModel;

        public LoadIncomePaymentsCommand(PeriodReportStore periodReportStore, IncomeReportViewModel incomeReportViewModel)
        {
            _periodReportStore = periodReportStore;
            _incomeReportViewModel = incomeReportViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _periodReportStore.GetPayments(_incomeReportViewModel.DateFrom, _incomeReportViewModel.DateTo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
