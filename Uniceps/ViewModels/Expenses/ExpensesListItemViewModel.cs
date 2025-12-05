using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.ExpensesCommands;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.Views.Expenses;
using Uniceps.Views.PlayerViews;
using Exp = Uniceps.Core.Models.Expenses;

namespace Uniceps.ViewModels.Expenses
{
    public class ExpensesListItemViewModel : ViewModelBase
    {
        public Core.Models.Expenses.Expenses? Expenses { get; set; }
        private readonly ExpensesDataStore? _expensesDataStore;
        private readonly NavigationStore? _navigatorStore;
        private readonly ExpensesListViewModel? _expensesListViewModel;
        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }
        public ExpensesListItemViewModel(Core.Models.Expenses.Expenses expenses, NavigationStore navigatorStore, ExpensesDataStore expensesDataStore, ExpensesListViewModel expensesListViewModel)
        {
            Expenses = expenses;
            _expensesDataStore = expensesDataStore;
            _navigatorStore = navigatorStore;
            _expensesListViewModel = expensesListViewModel;

            EditExpensesCommand = new RelayCommand(ExecuteEditExpensesCommand);
            DeleteExpensesCommand = new DeleteExpaensesCommand(expensesDataStore);
        }
        public void ExecuteEditExpensesCommand()
        {
            EditExpenseViewModel editExpenseViewModel = new EditExpenseViewModel(_expensesDataStore!,this);
            ExpenseDetailViewWinow expenseDetailViewWinow = new ExpenseDetailViewWinow();
            expenseDetailViewWinow.DataContext = editExpenseViewModel;
            expenseDetailViewWinow.ShowDialog();
        }
        public ExpensesListItemViewModel(Core.Models.Expenses.Expenses expenses, NavigationStore navigatorStore)
        {
            Expenses = expenses;
            _navigatorStore = navigatorStore;

        }
        public ExpensesListItemViewModel(Core.Models.Expenses.Expenses expenses)
        {
            Expenses = expenses;

        }
        public int Id => Expenses!.Id;
        public string? Description => Expenses!.Description;
        public double? Value => Expenses!.Value;
        public string date => Expenses!.date.ToShortDateString();

        public ICommand? EditExpensesCommand { get; }
        public ICommand? DeleteExpensesCommand { get; }
        public void Update(Core.Models.Expenses.Expenses obj)
        {
            Expenses = obj;
        }
    }
}
