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
using Unicepse.Stores.AccountantStores;

namespace Unicepse.ViewModels.Accountant
{
    public class AccountingViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly ExpensesDataStore _expensesStore;
        private readonly GymStore _gymStore;
        private readonly PaymentsDailyAccountantStore _paymentsDailyAccountantStore;
        private readonly ExpansesDailyAccountantDataStore _expansesDailyAccountantDataStore;
        private readonly SubscriptionDailyAccountantDataStore _subscriptionDailyAccountantDataStore;
        private readonly CreditsDailyAccountantStore _creditsDailyAccountantStore;
        private readonly AccountantDailyStore _accountantDailyStore;
        private readonly ExpensesAccountantDataStore _expensesAccountantDataStore;
        private readonly PaymentAccountantDataStore _paymentAccountantDataStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public ICommand ExpensesCommand { get; }
        public ICommand IncomeReportCommand { get; }
        public ICommand ExpensesReportCommand { get; }
        public ICommand StatesReportCommand { get; }
        //public ICommand PaymentReportCommand;
        public ICommand MonthlyIncomeReportCommand { get; }
        public AccountingViewModel( ExpensesDataStore expensesStore, GymStore gymStore, PaymentsDailyAccountantStore paymentsDailyAccountantStore, ExpansesDailyAccountantDataStore expansesDailyAccountantDataStore, SubscriptionDailyAccountantDataStore subscriptionDailyAccountantDataStore, CreditsDailyAccountantStore creditsDailyAccountantStore, AccountantDailyStore accountantDailyStore, ExpensesAccountantDataStore expensesAccountantDataStore, PaymentAccountantDataStore paymentAccountantDataStore)
        {
            _navigatorStore = new NavigationStore();
            _paymentsDailyAccountantStore = paymentsDailyAccountantStore;
            _expansesDailyAccountantDataStore = expansesDailyAccountantDataStore;
            _subscriptionDailyAccountantDataStore = subscriptionDailyAccountantDataStore;
            _creditsDailyAccountantStore = creditsDailyAccountantStore;
            _accountantDailyStore = accountantDailyStore;
            _expensesAccountantDataStore = expensesAccountantDataStore;
            _paymentAccountantDataStore = paymentAccountantDataStore;

            _expensesStore = expensesStore;
            _gymStore = gymStore;
            NavigationStore navigation = new NavigationStore();
            _navigatorStore.CurrentViewModel = CreateStatesViewModel(navigation, _expensesStore, _accountantDailyStore, _paymentsDailyAccountantStore, _expansesDailyAccountantDataStore, _subscriptionDailyAccountantDataStore, _creditsDailyAccountantStore);
            _navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            ExpensesCommand = new NavaigateCommand<ExpensesListViewModel>(new NavigationService<ExpensesListViewModel>(_navigatorStore, () => CreateExpenses(_navigatorStore, _expensesStore)));
            IncomeReportCommand = new NavaigateCommand<IncomeReportViewModel>(new NavigationService<IncomeReportViewModel>(_navigatorStore, () => new IncomeReportViewModel(_paymentAccountantDataStore, _navigatorStore)));
            ExpensesReportCommand = new NavaigateCommand<ExpensesReportViewModel>(new NavigationService<ExpensesReportViewModel>(_navigatorStore, () => new ExpensesReportViewModel(_expensesAccountantDataStore, _navigatorStore)));
            MonthlyIncomeReportCommand = new NavaigateCommand<MounthlyReportViewModel>(new NavigationService<MounthlyReportViewModel>(_navigatorStore, () => new MounthlyReportViewModel(_gymStore)));
            StatesReportCommand = new NavaigateCommand<AccountingStateViewModel>(new NavigationService<AccountingStateViewModel>(_navigatorStore, () => CreateStatesViewModel(navigation, _expensesStore, _accountantDailyStore, _paymentsDailyAccountantStore, _expansesDailyAccountantDataStore, _subscriptionDailyAccountantDataStore, _creditsDailyAccountantStore)));
        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private AccountingStateViewModel CreateStatesViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore, AccountantDailyStore gymStore, PaymentsDailyAccountantStore paymentsDailyAccountantStore,ExpansesDailyAccountantDataStore expansesDailyAccountantDataStore,SubscriptionDailyAccountantDataStore subscriptionDailyAccountantDataStore, CreditsDailyAccountantStore creditsDailyAccountantStore)
        {
            return AccountingStateViewModel.LoadViewModel(navigatorStore, expensesStore, gymStore, paymentsDailyAccountantStore, expansesDailyAccountantDataStore, subscriptionDailyAccountantDataStore, creditsDailyAccountantStore);
        }
        private ExpensesListViewModel CreateExpenses(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            return ExpensesListViewModel.LoadViewModel(navigatorStore, expensesStore);
        }

    }
}
