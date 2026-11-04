using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.ExpensesCommands;
using Uniceps.Commands.Player;
using Uniceps.Commands.RevenueCommands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.OtherRevenueViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views.Expenses;
using Exp = Uniceps.Core.Models.Expenses;

namespace Uniceps.ViewModels.Expenses
{
    public class ExpensesListViewModel : ListingViewModelBase
    {
        private readonly ExpensesDataStore _expensesStore;


        private readonly ObservableCollection<ExpensesListItemViewModel> expensesListItemViewModels;
        public IEnumerable<ExpensesListItemViewModel> ExpenseList => expensesListItemViewModels;
        public ICommand? AddExpensesCommand { get; }
        public bool HasData => expensesListItemViewModels.Count > 0;

        public ExpensesListViewModel(ExpensesDataStore expensesStore)
        {
            _expensesStore = expensesStore;
            LoadExpenseCommand = new LoadExpensesCommand(_expensesStore, this);
            expensesListItemViewModels = new ObservableCollection<ExpensesListItemViewModel>();


            _expensesStore.Loaded += _expensesStore_Loaded;
            _expensesStore.Created += _expensesStore_Created;
            _expensesStore.Updated += _expensesStore_Updated;
            _expensesStore.Deleted += _expensesStore_Deleted;
            AddExpensesCommand = new RelayCommand(ExecuteAddExpensesCommand);
            LoadExpenseCommand.Execute(null);
        }
        private void ExecuteAddExpensesCommand()
        {
            AddExpenseViewModel addExpenseViewModel = new AddExpenseViewModel(_expensesStore);
            ExpenseDetailViewWinow expenseDetailViewWinow = new ExpenseDetailViewWinow();
            expenseDetailViewWinow.DataContext = addExpenseViewModel;
            expenseDetailViewWinow.ShowDialog();
        }
        public ExpensesListItemViewModel? SelectedExpenses
        {
            get
            {
                return ExpenseList
                    .FirstOrDefault(y => y?.Expenses == _expensesStore.SelectedExpenses);
            }
            set
            {
                _expensesStore.SelectedExpenses = value?.Expenses;

            }
        }

        private void _expensesStore_Deleted(int obj)
        {
            ExpensesListItemViewModel? itemViewModel = expensesListItemViewModels.FirstOrDefault(y => y.Expenses?.Id == obj);

            if (itemViewModel != null)
            {
                expensesListItemViewModels.Remove(itemViewModel);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _expensesStore_Updated(Core.Models.Expenses.Expenses obj)
        {
            ExpensesListItemViewModel? expensesViewModel =
                    expensesListItemViewModels.FirstOrDefault(y => y.Expenses!.Id == obj.Id);

            if (expensesViewModel != null)
            {
                expensesViewModel.Update(obj);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _expensesStore_Created(Core.Models.Expenses.Expenses obj)
        {
            AddExpenses(obj);
        }

        private void _expensesStore_Loaded()
        {
            expensesListItemViewModels.Clear();
            foreach (Core.Models.Expenses.Expenses player in _expensesStore.Expenses)
            {
                AddExpenses(player);
            }

        }
        private void AddExpenses(Core.Models.Expenses.Expenses expenses)
        {
            ExpensesListItemViewModel itemViewModel =
                new ExpensesListItemViewModel(expenses, _expensesStore);
            expensesListItemViewModels.Add(itemViewModel);
            OnPropertyChanged(nameof(HasData));
        }



        public ICommand LoadExpenseCommand { get; private set; }

     
       
        public override void Dispose()
        {
            _expensesStore.Loaded -= _expensesStore_Loaded;
            _expensesStore.Created -= _expensesStore_Created;
            _expensesStore.Updated -= _expensesStore_Updated;
            _expensesStore.Deleted -= _expensesStore_Deleted;
            base.Dispose();
        }
    }
}
