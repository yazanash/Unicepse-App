﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Expenses
{
    public class ExpensesViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly ExpensesDataStore _expensesStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public ExpensesViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            _navigatorStore = navigatorStore;
            _expensesStore = expensesStore;
            navigatorStore.CurrentViewModel = CreateExpenseViewModel(_navigatorStore, _expensesStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;

        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private ExpensesListViewModel CreateExpenseViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            return ExpensesListViewModel.LoadViewModel(navigatorStore, expensesStore);
        }
    }
}
