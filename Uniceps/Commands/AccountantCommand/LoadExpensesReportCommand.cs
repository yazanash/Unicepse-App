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
    public class LoadExpensesReportCommand : AsyncCommandBase
    {
        private readonly PeriodReportStore _periodReportStore;
        private readonly ExpensesReportViewModel _expensesReportViewModel;

        public LoadExpensesReportCommand(PeriodReportStore periodReportStore, ExpensesReportViewModel expensesReportViewModel)
        {
            _periodReportStore = periodReportStore;
            _expensesReportViewModel = expensesReportViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _periodReportStore.GetExpenses(_expensesReportViewModel.DateFrom, _expensesReportViewModel.DateTo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
