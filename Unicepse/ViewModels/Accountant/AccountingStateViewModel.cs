using Unicepse.Commands.AccountantCommand;
using Unicepse.Commands.ExpensesCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Exp = Unicepse.Core.Models.Expenses;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.navigation.Stores;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.utlis.common;
using Unicepse.Stores.AccountantStores;

namespace Unicepse.ViewModels.Accountant
{
    public class AccountingStateViewModel : ListingViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ExpensesDataStore _expensesStore;
        private readonly AccountantDailyStore _gymStore;
        private readonly PaymentsDailyAccountantStore _paymentsDailyAccountantStore;
        private readonly ExpansesDailyAccountantDataStore _expansesDailyAccountantDataStore;
        private readonly SubscriptionDailyAccountantDataStore _subscriptionDailyAccountantDataStore;
        private readonly CreditsDailyAccountantStore _creditsDailyAccountantStore;
        private int ViewNum = 0;
        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;
        public ICommand SubscriptionsCommand { get; }
        public ICommand ExpensesCommand { get; }
        public ICommand IncomeCommand { get; }
        public ICommand CreditsCommand { get; }

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

        private double _subscriptionsCard;
        public double SubscriptionsCard
        {
            get { return _subscriptionsCard; }
            set { _subscriptionsCard = value; OnPropertyChanged(nameof(SubscriptionsCard)); }
        }

        private double _paymentsCard;
        public double PaymentsCard
        {
            get { return _paymentsCard; }
            set { _paymentsCard = value; OnPropertyChanged(nameof(PaymentsCard)); }
        }
        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
                SwitchViewModel();
            }
        }
        void SwitchViewModel()
        {
            LoadStateCommand.Execute(null);
            switch (ViewNum)
            {
                case 0:
                    ExpensesCommand.Execute(null);
                    break;
                case 1:
                    IncomeCommand.Execute(null);
                    break;
                case 2:
                    CreditsCommand.Execute(null);
                    break;
                case 3:
                    SubscriptionsCommand.Execute(null);
                    break;
            }
        }
        public AccountingStateViewModel(NavigationStore navigationStore, ExpensesDataStore expensesStore, AccountantDailyStore gymStore, PaymentsDailyAccountantStore paymentsDailyAccountantStore, ExpansesDailyAccountantDataStore expansesDailyAccountantDataStore, SubscriptionDailyAccountantDataStore subscriptionDailyAccountantDataStore, CreditsDailyAccountantStore creditsDailyAccountantStore)
        {
            _navigationStore = navigationStore;
            _expensesStore = expensesStore;
            _gymStore = gymStore;
            _paymentsDailyAccountantStore = paymentsDailyAccountantStore;
            _expansesDailyAccountantDataStore = expansesDailyAccountantDataStore;
            _subscriptionDailyAccountantDataStore = subscriptionDailyAccountantDataStore;
            _creditsDailyAccountantStore = creditsDailyAccountantStore;

            LoadStateCommand = new LoadStatesCommand(this, _expensesStore, _gymStore);
            _gymStore.DailyPaymentsSumLoaded += _gymStore_PaymentsLoaded;
            _gymStore.DailySubscriptionsSumLoaded += _gymStore_DailySubscriptionsSumLoaded;
            _gymStore.DailyExpensesSumLoaded += _gymStore_ExpensesLoaded;
            _gymStore.DailyCreditsSumLoaded += _gymStore_CreditsLoaded;
            _navigationStore.CurrentViewModel = CreateSubscriptions(_subscriptionDailyAccountantDataStore, this);
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
            ExpensesCommand = new NavaigateCommand<ExpensesCardViewModel>(new NavigationService<ExpensesCardViewModel>(_navigationStore, () => CreateExpenses(_expansesDailyAccountantDataStore, this)));
            IncomeCommand = new NavaigateCommand<PaymentsCardViewModel>(new NavigationService<PaymentsCardViewModel>(_navigationStore, () => CreatePayments(_paymentsDailyAccountantStore, this)));
            CreditsCommand = new NavaigateCommand<CreditsCardViewModel>(new NavigationService<CreditsCardViewModel>(_navigationStore, () => CreateCredits(_creditsDailyAccountantStore, this)));
            SubscriptionsCommand = new NavaigateCommand<SubscriptionCardViewModel>(new NavigationService<SubscriptionCardViewModel>(_navigationStore, () => CreateSubscriptions(_subscriptionDailyAccountantDataStore, this)));
        }

        private void _gymStore_DailySubscriptionsSumLoaded(double obj)
        {
            SubscriptionsCard = obj;
        }

        private void _navigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void _gymStore_CreditsLoaded(double obj)
        {
            CreditsCard = obj;
        }

        private void _gymStore_ExpensesLoaded(double obj)
        {
            ExpensesCard = obj;
        }

        private void _gymStore_PaymentsLoaded(double obj)
        {
            PaymentsCard = obj;
        }
        public ICommand LoadStateCommand;
        public static AccountingStateViewModel LoadViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore, AccountantDailyStore gymStore, PaymentsDailyAccountantStore paymentsDailyAccountantStore, ExpansesDailyAccountantDataStore expansesDailyAccountantDataStore,SubscriptionDailyAccountantDataStore subscriptionDailyAccountantDataStore, CreditsDailyAccountantStore creditsDailyAccountantStore)
        {
            AccountingStateViewModel viewModel = new AccountingStateViewModel(navigatorStore, expensesStore, gymStore, paymentsDailyAccountantStore, expansesDailyAccountantDataStore, subscriptionDailyAccountantDataStore, creditsDailyAccountantStore);

            viewModel.LoadStateCommand.Execute(null);

            return viewModel;
        }

        private ExpensesCardViewModel CreateExpenses(ExpansesDailyAccountantDataStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 0;
            return ExpensesCardViewModel.LoadViewModel(gymStore, accountingStateViewModel);
        }
        private PaymentsCardViewModel CreatePayments(PaymentsDailyAccountantStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 1;
            return PaymentsCardViewModel.LoadViewModel(gymStore, accountingStateViewModel);
        }
        private CreditsCardViewModel CreateCredits(CreditsDailyAccountantStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 2;
            return CreditsCardViewModel.LoadViewModel(gymStore, accountingStateViewModel);
        }
        private SubscriptionCardViewModel CreateSubscriptions(SubscriptionDailyAccountantDataStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 3;
            return SubscriptionCardViewModel.LoadViewModel(gymStore, accountingStateViewModel);
        }
    }
}
