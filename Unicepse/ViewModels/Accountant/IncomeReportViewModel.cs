using Unicepse.Core.Models.Payment;
using Unicepse.Commands.AccountantCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.ViewModels;
using Unicepse.Stores;
using Unicepse.navigation.Stores;
using Unicepse.Stores.AccountantStores;

namespace Unicepse.ViewModels.Accountant
{
    public class IncomeReportViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<IncomeListItemViewModel> _incomeListItemViewModels;
        private readonly PaymentAccountantDataStore _paymentDataStore;
        private readonly NavigationStore _navigationStore;
        public IEnumerable<IncomeListItemViewModel> IncomeList => _incomeListItemViewModels;
        public IncomeReportViewModel(PaymentAccountantDataStore paymentDataStore, NavigationStore navigationStore)
        {
            _paymentDataStore = paymentDataStore;
            _navigationStore = navigationStore;
            _paymentDataStore.PaymentsLoaded += _paymentDataStore_Loaded;
            _incomeListItemViewModels = new ObservableCollection<IncomeListItemViewModel>();
            LoadPayments = new LoadIncomePaymentsCommand(_paymentDataStore, this);
        }
        public ICommand LoadPayments { get; }
        private void _paymentDataStore_Loaded()
        {
            _incomeListItemViewModels.Clear();
            foreach (var payment in _paymentDataStore.Payments.OrderByDescending(x => x.PayDate))
            {
                AddPayment(payment);
            }
            Total = _paymentDataStore.Payments.Sum(x=>x.PaymentValue);
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
