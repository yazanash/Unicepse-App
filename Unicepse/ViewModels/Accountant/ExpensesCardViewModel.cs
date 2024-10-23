using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.AccountantCommand;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Exp = Unicepse.Core.Models.Expenses;
namespace Unicepse.ViewModels.Accountant
{
    public class ExpensesCardViewModel : ListingViewModelBase
    {
        private readonly GymStore _gymStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public ExpensesCardViewModel(GymStore gymStore, AccountingStateViewModel accountingStateViewModel)
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
            foreach (Exp.Expenses player in _gymStore.Expenses)
            {
                AddExpenses(player);
            }
        }

        private readonly ObservableCollection<Expenses.ExpensesListItemViewModel> expensesListItemViewModels;
        public IEnumerable<Expenses.ExpensesListItemViewModel> ExpenseList => expensesListItemViewModels;

        private void AddExpenses(Exp.Expenses expenses)
        {
            Expenses.ExpensesListItemViewModel itemViewModel =
                 new Expenses.ExpensesListItemViewModel(expenses);
            expensesListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = expensesListItemViewModels.Count();
        }
        public ICommand LoadExpensesCommand;
        public static ExpensesCardViewModel LoadViewModel(GymStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            ExpensesCardViewModel viewModel = new ExpensesCardViewModel(gymStore,accountingStateViewModel);

            viewModel.LoadExpensesCommand.Execute(null);

            return viewModel;
        }
    }
}
