using Unicepse.Core.Models.Expenses;
using Unicepse.WPF.Commands;
using Unicepse.WPF.Commands.ExpensesCommands;
using Unicepse.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Exp = Unicepse.Core.Models.Expenses;
using Unicepse.WPF.navigation.Stores;
using Unicepse.WPF.utlis.common;
using Unicepse.WPF.Commands.Player;
using Unicepse.WPF.navigation;
namespace Unicepse.WPF.ViewModels.Expenses
{
    public class ExpensesListItemViewModel : ViewModelBase
    {
        public Exp.Expenses Expenses { get; set; }
        private readonly ExpensesDataStore _expensesDataStore;
        private readonly NavigationStore _navigatorStore;
        private readonly ExpensesListViewModel _expensesListViewModel;

        public ExpensesListItemViewModel(Exp.Expenses expenses, NavigationStore navigatorStore, ExpensesDataStore expensesDataStore, ExpensesListViewModel expensesListViewModel)
        {
            Expenses = expenses;
            _expensesDataStore = expensesDataStore;
            _navigatorStore = navigatorStore;
            _expensesListViewModel = expensesListViewModel;

            EditExpensesCommand = new NavaigateCommand<EditExpenseViewModel>(new NavigationService<EditExpenseViewModel>(_navigatorStore, () => new EditExpenseViewModel(_expensesDataStore, _navigatorStore, _expensesListViewModel)));
            DeleteExpensesCommand = new DeleteExpaensesCommand(expensesDataStore);
        }

        public ExpensesListItemViewModel(Exp.Expenses expenses, NavigationStore navigatorStore)
        {
            Expenses = expenses;
            _navigatorStore = navigatorStore;

        }
        public int Id => Expenses.Id;
        public string? Description => Expenses.Description;
        public double? Value => Expenses.Value;
        public string date => Expenses.date.ToShortDateString();

        public ICommand EditExpensesCommand { get; }
        public ICommand DeleteExpensesCommand { get; }
        public void Update(Exp.Expenses obj)
        {
            this.Expenses = obj;
        }
    }
}
