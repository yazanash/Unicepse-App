using PlatinumGymPro.Commands.AccountantCommand;
using PlatinumGymPro.Commands.ExpensesCommands;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Expenses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Exp = PlatinumGym.Core.Models.Expenses;
namespace PlatinumGymPro.ViewModels.Accountant
{
    public class AccountingStateViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ExpensesDataStore _expensesStore;
        private readonly ObservableCollection<Expenses.ExpensesListItemViewModel> expensesListItemViewModels;
        public IEnumerable<Expenses.ExpensesListItemViewModel> ExpenseList => expensesListItemViewModels;
        public AccountingStateViewModel(NavigationStore navigationStore,ExpensesDataStore expensesStore)
        {
            _navigationStore = navigationStore;
            _expensesStore = expensesStore;
            expensesListItemViewModels = new ObservableCollection<Expenses.ExpensesListItemViewModel>();
            LoadStateCommand = new LoadStatesCommand(this, _expensesStore);
            _expensesStore.Loaded += _expensesStore_Loaded;
        }

        private void _expensesStore_Loaded()
        {
            expensesListItemViewModels.Clear();
            foreach (Exp.Expenses player in _expensesStore.Expenses)
            {
                AddExpenses(player);
            }
        }
        private void AddExpenses(Exp.Expenses expenses)
        {
           Expenses.ExpensesListItemViewModel itemViewModel =
                new Expenses.ExpensesListItemViewModel(expenses, _navigationStore);
            expensesListItemViewModels.Add(itemViewModel);
        }
        public ICommand LoadStateCommand;
        public static AccountingStateViewModel LoadViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            AccountingStateViewModel viewModel = new AccountingStateViewModel(navigatorStore, expensesStore);

            viewModel.LoadStateCommand.Execute(null);

            return viewModel;
        }
    }
}
