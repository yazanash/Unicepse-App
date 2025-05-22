using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Stores.AccountantStores;
using Uniceps.ViewModels;
using Uniceps.Commands.AccountantCommand;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Exp = Uniceps.Core.Models.Expenses;
namespace Uniceps.ViewModels.Accountant
{
    public class ExpensesCardViewModel : ListingViewModelBase
    {
        private readonly ExpansesDailyAccountantDataStore _gymStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public ExpensesCardViewModel(ExpansesDailyAccountantDataStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            _gymStore = gymStore;
            _accountingStateViewModel = accountingStateViewModel;
            expensesListItemViewModels = new ObservableCollection<Expenses.ExpensesListItemViewModel>();
            LoadExpensesCommand = new LoadDailyExpenses(_gymStore, _accountingStateViewModel);
            _gymStore.ExpensesLoaded += _gymStore_ExpensesLoaded;

        }

        private void _gymStore_ExpensesLoaded()
        {
            expensesListItemViewModels.Clear();
            foreach (Core.Models.Expenses.Expenses player in _gymStore.Expenses)
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
        public static ExpensesCardViewModel LoadViewModel(ExpansesDailyAccountantDataStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            ExpensesCardViewModel viewModel = new ExpensesCardViewModel(gymStore, accountingStateViewModel);

            viewModel.LoadExpensesCommand.Execute(null);

            return viewModel;
        }
    }
}
