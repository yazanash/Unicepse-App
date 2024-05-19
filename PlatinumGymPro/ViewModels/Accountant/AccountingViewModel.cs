using PlatinumGymPro.Commands;
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
        private readonly PaymentDataStore _paymentDataStore ;
        private readonly GymStore _gymStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public ICommand ExpensesCommand { get; }
        public ICommand IncomeReportCommand { get; }
        public ICommand ExpensesReportCommand { get; }
        //public ICommand PaymentReportCommand;
        public ICommand MonthlyIncomeReportCommand { get; }
        public AccountingViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore, PaymentDataStore paymentDataStore, GymStore gymStore)
        {
            _navigatorStore = navigatorStore;
            _expensesStore = expensesStore;
            _gymStore = gymStore;
            _paymentDataStore = paymentDataStore;
            navigatorStore.CurrentViewModel = CreateStatesViewModel(_navigatorStore, _expensesStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            ExpensesCommand = new NavaigateCommand<ExpensesListViewModel>(new NavigationService<ExpensesListViewModel>(_navigatorStore, () => CreateExpenses(_navigatorStore, _expensesStore)));
            IncomeReportCommand = new NavaigateCommand<IncomeReportViewModel>(new NavigationService<IncomeReportViewModel>(_navigatorStore, () => new IncomeReportViewModel(_paymentDataStore, _navigatorStore)));
            ExpensesReportCommand = new NavaigateCommand<ExpensesReportViewModel>(new NavigationService<ExpensesReportViewModel>(_navigatorStore, () => new ExpensesReportViewModel(_expensesStore, _navigatorStore)));
            MonthlyIncomeReportCommand = new NavaigateCommand<MounthlyReportViewModel>(new NavigationService<MounthlyReportViewModel>(_navigatorStore, () => new MounthlyReportViewModel(_gymStore)));

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
