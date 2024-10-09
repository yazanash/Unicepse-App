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

        public ExpensesCardViewModel(GymStore gymStore)
        {
            _gymStore = gymStore;
            expensesListItemViewModels = new ObservableCollection<Expenses.ExpensesListItemViewModel>();
            LoadExpensesCommand = new LoadDailyExpenses(_gymStore);
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
        public static ExpensesCardViewModel LoadViewModel(GymStore gymStore)
        {
            ExpensesCardViewModel viewModel = new ExpensesCardViewModel(gymStore);

            viewModel.LoadExpensesCommand.Execute(null);

            return viewModel;
        }
    }
}
