using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Stores;
using Uniceps.Commands;
using Uniceps.ViewModels.Accountant;
using Uniceps.Stores.AccountantStores;

namespace Uniceps.Commands.AccountantCommand
{
    public class LoadIncomePaymentsCommand : AsyncCommandBase
    {
        private readonly PaymentAccountantDataStore _paymentDataStore;
        private readonly IncomeReportViewModel _incomeReportViewModel;

        public LoadIncomePaymentsCommand(PaymentAccountantDataStore paymentDataStore, IncomeReportViewModel incomeReportViewModel)
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
