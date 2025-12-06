using Uniceps.Commands.AccountantCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.navigation.Stores;
using Uniceps.Core.Models.Payment;

namespace Uniceps.ViewModels.Accountant
{
    public class IncomeReportViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<IncomeListItemViewModel> _incomeListItemViewModels;
        private readonly PeriodReportStore _periodReportStore;
        public IEnumerable<IncomeListItemViewModel> IncomeList => _incomeListItemViewModels;
        public IncomeReportViewModel(PeriodReportStore periodReportStore)
        {
            _periodReportStore = periodReportStore;
            _periodReportStore.PaymentLoaded += _periodReportStore_PaymentLoaded;
            _incomeListItemViewModels = new ObservableCollection<IncomeListItemViewModel>();
            LoadPayments = new LoadIncomePaymentsCommand(_periodReportStore, this);
        }
        public ICommand? LoadPayments { get; }
        private void _periodReportStore_PaymentLoaded()
        {
            _incomeListItemViewModels.Clear();
            foreach (var payment in _periodReportStore.Payments.OrderByDescending(x => x.PayDate))
            {
                AddPayment(payment);
            }
            Total = _periodReportStore.Payments.Sum(x => x.PaymentValue);
        }
        private void AddPayment(PlayerPayment playerPayment)
        {
            IncomeListItemViewModel incomeListItemViewModel = new IncomeListItemViewModel(playerPayment);
            _incomeListItemViewModels.Add(incomeListItemViewModel);
            incomeListItemViewModel.Order = _incomeListItemViewModels.Count();
        }
        private double _total = 0;
        public double Total
        {
            get { return _total; }
            set { _total = value; OnPropertyChanged(nameof(Total)); }
        }

        private DateTime _dateFrom = DateTime.Now;
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { _dateFrom = value; OnPropertyChanged(nameof(DateFrom)); }
        }

        private DateTime _dateTo = DateTime.Now;
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { _dateTo = value; OnPropertyChanged(nameof(DateTo)); }
        }
    }
}
