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
        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }
        public ExpensesListItemViewModel(Core.Models.Expenses.Expenses expenses, ExpensesDataStore expensesDataStore)
        {
            Expenses = expenses;
            _expensesDataStore = expensesDataStore;

            EditExpensesCommand = new RelayCommand(ExecuteEditExpensesCommand);
            DeleteExpensesCommand = new DeleteExpensesCommand(expensesDataStore);
        }
        public void ExecuteEditExpensesCommand()
        {
            AddExpenseViewModel editExpenseViewModel = new AddExpenseViewModel(_expensesDataStore!,Expenses!);
            ExpenseDetailViewWinow expenseDetailViewWinow = new ExpenseDetailViewWinow();
            expenseDetailViewWinow.DataContext = editExpenseViewModel;
            expenseDetailViewWinow.ShowDialog();
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
            OnPropertyChanged(nameof(Expenses));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(date));
        }
    }
}
