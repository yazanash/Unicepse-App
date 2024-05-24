using Unicepse.Core.Models.Payment;
using Unicepse.WPF.Commands.AccountantCommand;
using Unicepse.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.WPF.navigation.Stores;

namespace Unicepse.WPF.ViewModels.Accountant
{
    public class IncomeReportViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<IncomeListItemViewModel> _incomeListItemViewModels;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly NavigationStore _navigationStore;
        public IEnumerable<IncomeListItemViewModel> IncomeList => _incomeListItemViewModels;
        public IncomeReportViewModel(PaymentDataStore paymentDataStore, NavigationStore navigationStore)
        {
            _paymentDataStore = paymentDataStore;
            _navigationStore = navigationStore;
            _paymentDataStore.Loaded += _paymentDataStore_Loaded;
            _incomeListItemViewModels = new ObservableCollection<IncomeListItemViewModel>();
            LoadPayments = new LoadIncomePaymentsCommand(_paymentDataStore, this);
        }
        public ICommand LoadPayments { get; }
        private void _paymentDataStore_Loaded()
        {
            _incomeListItemViewModels.Clear();
            foreach(var payment in _paymentDataStore.Payments.OrderByDescending(x=>x.PayDate))
            {
                AddPayment(payment);
            }
            Total = _paymentDataStore.GetSum();
        }
        private void AddPayment(PlayerPayment playerPayment)
        {
            IncomeListItemViewModel incomeListItemViewModel = new IncomeListItemViewModel(playerPayment);
            _incomeListItemViewModels.Add(incomeListItemViewModel);
        }
        private double _total = 0;
        public double Total
        {
            get { return _total; }
            set { _total = value; OnPropertyChanged(nameof(Total)); }
        }

        private DateTime _dateFrom=DateTime.Now;
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
