using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.Payment;
using Uniceps.ViewModels;
using Uniceps.Commands.AccountantCommand;
using Uniceps.Stores;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.Accountant
{
    public class PaymentsCardViewModel : ListingViewModelBase
    {
        private readonly DailyReportStore _dailyReportStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public PaymentsCardViewModel(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            _dailyReportStore = dailyReportStore;
            _accountingStateViewModel = accountingStateViewModel;
            _incomeListItemViewModels = new ObservableCollection<IncomeListItemViewModel>();
            LoadPaymentsCommand = new LoadDailyPayments(_dailyReportStore, _accountingStateViewModel);
            _dailyReportStore.PaymentLoaded +=_dailyReportStore_PaymentLoaded;

        }

        private void _dailyReportStore_PaymentLoaded()
        {
            _incomeListItemViewModels.Clear();
            foreach (PlayerPayment payment in _dailyReportStore.Payments)
            {
                AddPayment(payment);
            }
        }

        private readonly ObservableCollection<IncomeListItemViewModel> _incomeListItemViewModels;
        public IEnumerable<IncomeListItemViewModel> PaymentsList => _incomeListItemViewModels;

        private void AddPayment(PlayerPayment payments)
        {
            IncomeListItemViewModel itemViewModel =
                 new IncomeListItemViewModel(payments);
            _incomeListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _incomeListItemViewModels.Count();
        }
        public ICommand LoadPaymentsCommand;
        public static PaymentsCardViewModel LoadViewModel(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            PaymentsCardViewModel viewModel = new PaymentsCardViewModel(dailyReportStore, accountingStateViewModel);

            viewModel.LoadPaymentsCommand.Execute(null);

            return viewModel;
        }
    }
}
