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
using Uniceps.Stores.AccountantStores;

namespace Uniceps.ViewModels.Accountant
{
    public class ExpensesReportViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<ExpensesListItemViewModel> _expensesListItemViewModel;
        private readonly ExpensesAccountantDataStore _expensesDataStore;
        private readonly NavigationStore _navigationStore;
        public IEnumerable<ExpensesListItemViewModel> ExpensesList => _expensesListItemViewModel;
        public ExpensesReportViewModel(ExpensesAccountantDataStore expensesDataStore, NavigationStore navigationStore)
        {
            _expensesDataStore = expensesDataStore;
            _navigationStore = navigationStore;
            _expensesDataStore.ExpensesLoaded += _paymentDataStore_Loaded;
            _expensesListItemViewModel = new ObservableCollection<ExpensesListItemViewModel>();
            LoadExpenses = new LoadExpensesReportCommand(_expensesDataStore, this);
        }
        public ICommand LoadExpenses { get; }
        private void _paymentDataStore_Loaded()
        {
            _expensesListItemViewModel.Clear();
            foreach (var expenses in _expensesDataStore.Expenses.OrderByDescending(x => x.date))
            {
                AddExpenses(expenses);
            }
            Total = _expensesDataStore.Expenses.Sum(x => x.Value);
        }
        private void AddExpenses(Core.Models.Expenses.Expenses expenses)
        {
            ExpensesListItemViewModel incomeListItemViewModel = new ExpensesListItemViewModel(expenses);
            _expensesListItemViewModel.Add(incomeListItemViewModel);
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
