using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.ViewModels.Expenses;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Accountant
{
    public class AccountingViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly ExpensesDataStore _expensesStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly GymStore _gymStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public ICommand ExpensesCommand { get; }
        public ICommand IncomeReportCommand { get; }
        public ICommand ExpensesReportCommand { get; }
        public ICommand StatesReportCommand { get; }
        //public ICommand PaymentReportCommand;
        public ICommand MonthlyIncomeReportCommand { get; }
        public AccountingViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore, PaymentDataStore paymentDataStore, GymStore gymStore)
        {
            _navigatorStore = navigatorStore;
            _expensesStore = expensesStore;
            _gymStore = gymStore;
            _paymentDataStore = paymentDataStore;
            NavigationStore navigation = new NavigationStore();
            _navigatorStore.CurrentViewModel = CreateStatesViewModel(navigation, _expensesStore, _gymStore);
            _navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            ExpensesCommand = new NavaigateCommand<ExpensesListViewModel>(new NavigationService<ExpensesListViewModel>(_navigatorStore, () => CreateExpenses(_navigatorStore, _expensesStore)));
            IncomeReportCommand = new NavaigateCommand<IncomeReportViewModel>(new NavigationService<IncomeReportViewModel>(_navigatorStore, () => new IncomeReportViewModel(_paymentDataStore, _navigatorStore)));
            ExpensesReportCommand = new NavaigateCommand<ExpensesReportViewModel>(new NavigationService<ExpensesReportViewModel>(_navigatorStore, () => new ExpensesReportViewModel(_expensesStore, _navigatorStore)));
            MonthlyIncomeReportCommand = new NavaigateCommand<MounthlyReportViewModel>(new NavigationService<MounthlyReportViewModel>(_navigatorStore, () => new MounthlyReportViewModel(_gymStore)));
            StatesReportCommand = new NavaigateCommand<AccountingStateViewModel>(new NavigationService<AccountingStateViewModel>(_navigatorStore, () => CreateStatesViewModel(navigation, _expensesStore, _gymStore)));

        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private AccountingStateViewModel CreateStatesViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore, GymStore gymStore)
        {
            return AccountingStateViewModel.LoadViewModel(navigatorStore, expensesStore, gymStore);
        }
        private ExpensesListViewModel CreateExpenses(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            return ExpensesListViewModel.LoadViewModel(navigatorStore, expensesStore);
        }

    }
}
