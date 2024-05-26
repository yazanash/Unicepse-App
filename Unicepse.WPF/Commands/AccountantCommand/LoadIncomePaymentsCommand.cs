using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.Accountant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Unicepse.WPF.Commands.AccountantCommand
{
    public class LoadIncomePaymentsCommand : AsyncCommandBase
    {
        private readonly PaymentDataStore _paymentDataStore;
        private readonly IncomeReportViewModel _incomeReportViewModel;

        public LoadIncomePaymentsCommand(PaymentDataStore paymentDataStore, IncomeReportViewModel incomeReportViewModel)
        {
            _paymentDataStore = paymentDataStore;
            _incomeReportViewModel = incomeReportViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _paymentDataStore.GetAll(_incomeReportViewModel.DateFrom, _incomeReportViewModel.DateTo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
