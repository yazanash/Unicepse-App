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
    public class AccountingStateViewModel : ListingViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ExpensesDataStore _expensesStore;
        private readonly GymStore _gymStore;
        private readonly ObservableCollection<Expenses.ExpensesListItemViewModel> expensesListItemViewModels;
        private double _expensesCard;
        public double ExpensesCard
        {
            get { return _expensesCard; }
            set { _expensesCard = value; OnPropertyChanged(nameof(ExpensesCard)); }
        }
       private double _creditsCard;
        public double CreditsCard
        {
            get { return _creditsCard; }
            set { _creditsCard = value; OnPropertyChanged(nameof(CreditsCard)); }
        }
        private double _paymentsCard;
       public double PaymentsCard
        {
            get { return _paymentsCard; }
            set { _paymentsCard = value; OnPropertyChanged(nameof(PaymentsCard)); }
        }
        public IEnumerable<Expenses.ExpensesListItemViewModel> ExpenseList => expensesListItemViewModels;
        public AccountingStateViewModel(NavigationStore navigationStore, ExpensesDataStore expensesStore, GymStore gymStore)
        {
            _navigationStore = navigationStore;
            _expensesStore = expensesStore;
            _gymStore = gymStore;
            expensesListItemViewModels = new ObservableCollection<Expenses.ExpensesListItemViewModel>();
            LoadStateCommand = new LoadStatesCommand(this, _expensesStore,_gymStore);
            _expensesStore.Loaded += _expensesStore_Loaded;
            _gymStore.DailyPaymentsSumLoaded += _gymStore_PaymentsLoaded;
            _gymStore.DailyExpensesSumLoaded += _gymStore_ExpensesLoaded;
            _gymStore.DailyCreditsSumLoaded += _gymStore_CreditsLoaded;
        }

        private void _gymStore_CreditsLoaded(double obj)
        {
            PaymentsCard = obj;
        }

        private void _gymStore_ExpensesLoaded(double obj)
        {
            ExpensesCard = obj;
        }

        private void _gymStore_PaymentsLoaded(double obj)
        {
            CreditsCard = obj;
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
        public static AccountingStateViewModel LoadViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore, GymStore gymStore)
        {
            AccountingStateViewModel viewModel = new AccountingStateViewModel(navigatorStore, expensesStore, gymStore);

            viewModel.LoadStateCommand.Execute(null);

            return viewModel;
        }
    }
}
