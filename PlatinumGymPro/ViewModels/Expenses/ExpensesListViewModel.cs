using PlatinumGymPro.Commands.ExpensesCommands;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Exp = PlatinumGym.Core.Models.Expenses ;

namespace PlatinumGymPro.ViewModels.Expenses
{
    public class ExpensesListViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigatorStore;
        private readonly ExpensesDataStore _expensesStore;


        private readonly ObservableCollection<ExpensesListItemViewModel> expensesListItemViewModels;
        public IEnumerable<ExpensesListItemViewModel> ExpenseList => expensesListItemViewModels;
        public ICommand? AddExpensesCommand { get; }

        public ExpensesListViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            _navigatorStore = navigatorStore;
            _expensesStore = expensesStore;
            LoadExpenseCommand = new LoadExpensesCommand(_expensesStore, this);
            expensesListItemViewModels = new ObservableCollection<ExpensesListItemViewModel>();


            _expensesStore.Loaded += _expensesStore_Loaded;
            _expensesStore.Created += _expensesStore_Created;
            _expensesStore.Updated += _expensesStore_Updated;
            _expensesStore.Deleted += _expensesStore_Deleted;
        }

        private void _expensesStore_Deleted(int obj)
        {
            ExpensesListItemViewModel? itemViewModel = expensesListItemViewModels.FirstOrDefault(y => y.Expenses?.Id == obj);

            if (itemViewModel != null)
            {
                expensesListItemViewModels.Remove(itemViewModel);
            }
        }
        
        private void _expensesStore_Updated(Exp.Expenses obj)
        {
            ExpensesListItemViewModel? expensesViewModel =
                    expensesListItemViewModels.FirstOrDefault(y => y.Expenses.Id == obj.Id);

            if (expensesViewModel != null)
            {
                expensesViewModel.Update(obj);
            }
        }

        private void _expensesStore_Created(Exp.Expenses obj)
        {
            AddExpenses(obj);
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
            ExpensesListItemViewModel itemViewModel =
                new ExpensesListItemViewModel(expenses, _navigatorStore);
            expensesListItemViewModels.Add(itemViewModel);
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }
        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);



        public ICommand LoadExpenseCommand { get; private set; }

        public static ExpensesListViewModel LoadViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            ExpensesListViewModel viewModel = new ExpensesListViewModel(navigatorStore, expensesStore);

            viewModel.LoadExpenseCommand.Execute(null);

            return viewModel;
        }

        protected override void Dispose()
        {
            _expensesStore.Loaded -= _expensesStore_Loaded;
            _expensesStore.Created -= _expensesStore_Created;
            _expensesStore.Updated -= _expensesStore_Updated;
            _expensesStore.Deleted -= _expensesStore_Deleted;
            base.Dispose();
        }
    }
}
