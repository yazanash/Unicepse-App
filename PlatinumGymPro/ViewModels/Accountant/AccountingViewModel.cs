﻿using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Expenses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.Accountant
{
    public class AccountingViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly ExpensesDataStore _expensesStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public ICommand ExpensesCommand { get; }
        //public ICommand DailyReportCommand;
        //public ICommand ExpensesReportCommand;
        //public ICommand PaymentReportCommand;
        //public ICommand IncomeReportCommand;
        public AccountingViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            _navigatorStore = navigatorStore;
            _expensesStore = expensesStore;
            navigatorStore.CurrentViewModel = CreateStatesViewModel(_navigatorStore, _expensesStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            ExpensesCommand = new NavaigateCommand<ExpensesListViewModel>(new NavigationService<ExpensesListViewModel>(_navigatorStore, () => CreateExpenses(_navigatorStore, _expensesStore)));
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private AccountingStateViewModel CreateStatesViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            return AccountingStateViewModel.LoadViewModel(navigatorStore, expensesStore);
        }
        private ExpensesListViewModel CreateExpenses(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            return ExpensesListViewModel.LoadViewModel(navigatorStore, expensesStore);
        }

    }
}
