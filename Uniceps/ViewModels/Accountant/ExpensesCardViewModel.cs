using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.ViewModels;
using Uniceps.Commands.AccountantCommand;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Exp = Uniceps.Core.Models.Expenses;
namespace Uniceps.ViewModels.Accountant
{
    public class ExpensesCardViewModel : ListingViewModelBase
    {
        private readonly DailyReportStore _dailyReportStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public ExpensesCardViewModel(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            _dailyReportStore = dailyReportStore;
            _accountingStateViewModel = accountingStateViewModel;
            expensesListItemViewModels = new ObservableCollection<Expenses.ExpensesListItemViewModel>();
            LoadExpensesCommand = new LoadDailyExpenses(_dailyReportStore, _accountingStateViewModel);
            _dailyReportStore.ExpensesLoaded += _dailyReportStore_ExpensesLoaded;

        }

        private void _dailyReportStore_ExpensesLoaded()
        {
            expensesListItemViewModels.Clear();
            foreach (Core.Models.Expenses.Expenses player in _dailyReportStore.Expenses)
            {
                AddExpenses(player);
            }
        }

        private readonly ObservableCollection<Expenses.ExpensesListItemViewModel> expensesListItemViewModels;
        public IEnumerable<Expenses.ExpensesListItemViewModel> ExpenseList => expensesListItemViewModels;

        private void AddExpenses(Core.Models.Expenses.Expenses expenses)
        {
            Expenses.ExpensesListItemViewModel itemViewModel =
                 new Expenses.ExpensesListItemViewModel(expenses);
            expensesListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = expensesListItemViewModels.Count();
        }
        public ICommand LoadExpensesCommand;
        public static ExpensesCardViewModel LoadViewModel(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            ExpensesCardViewModel viewModel = new ExpensesCardViewModel(dailyReportStore, accountingStateViewModel);

            viewModel.LoadExpensesCommand.Execute(null);

            return viewModel;
        }
    }
}
