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

namespace Uniceps.ViewModels.Accountant
{
    public class ExpensesReportViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<ExpensesListItemViewModel> _expensesListItemViewModel;
        private readonly PeriodReportStore _periodReportStore;
        public IEnumerable<ExpensesListItemViewModel> ExpensesList => _expensesListItemViewModel;
        public ExpensesReportViewModel(PeriodReportStore periodReportStore)
        {
            _periodReportStore = periodReportStore;
            _periodReportStore.ExpensesLoaded += __periodReportStore_ExpensesLoaded;
            _expensesListItemViewModel = new ObservableCollection<ExpensesListItemViewModel>();
            LoadExpenses = new LoadExpensesReportCommand(_periodReportStore, this);
        }
        public ICommand LoadExpenses { get; }
        private void __periodReportStore_ExpensesLoaded()
        {
            _expensesListItemViewModel.Clear();
            foreach (var expenses in _periodReportStore.Expenses.OrderByDescending(x => x.date))
            {
                AddExpenses(expenses);
            }
            Total = _periodReportStore.Expenses.Sum(x => x.Value);
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
