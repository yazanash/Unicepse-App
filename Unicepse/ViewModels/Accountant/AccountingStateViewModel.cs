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

namespace Unicepse.ViewModels.Accountant
{
    public class AccountingStateViewModel : ListingViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ExpensesDataStore _expensesStore;
        private readonly GymStore _gymStore;
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
        public AccountingStateViewModel(NavigationStore navigationStore, ExpensesDataStore expensesStore, GymStore gymStore)
        {
            _navigationStore = navigationStore;
            _expensesStore = expensesStore;
            _gymStore = gymStore;
            LoadStateCommand = new LoadStatesCommand(this, _expensesStore, _gymStore);
            _gymStore.DailyPaymentsSumLoaded += _gymStore_PaymentsLoaded;
            _gymStore.DailySubscriptionsSumLoaded += _gymStore_DailySubscriptionsSumLoaded; 
            _gymStore.DailyExpensesSumLoaded += _gymStore_ExpensesLoaded;
            _gymStore.DailyCreditsSumLoaded += _gymStore_CreditsLoaded;
            _navigationStore.CurrentViewModel = CreateSubscriptions(_gymStore, this);
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
            ExpensesCommand = new NavaigateCommand<ExpensesCardViewModel>(new NavigationService<ExpensesCardViewModel>(_navigationStore, () => CreateExpenses(_gymStore, this)));
            IncomeCommand = new NavaigateCommand<PaymentsCardViewModel>(new NavigationService<PaymentsCardViewModel>(_navigationStore, () => CreatePayments(_gymStore, this)));
            CreditsCommand = new NavaigateCommand<CreditsCardViewModel>(new NavigationService<CreditsCardViewModel>(_navigationStore, () => CreateCredits(_gymStore, this)));
            SubscriptionsCommand = new NavaigateCommand<SubscriptionCardViewModel>(new NavigationService<SubscriptionCardViewModel>(_navigationStore, () => CreateSubscriptions(_gymStore, this)));
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
        public static AccountingStateViewModel LoadViewModel(NavigationStore navigatorStore, ExpensesDataStore expensesStore, GymStore gymStore)
        {
            AccountingStateViewModel viewModel = new AccountingStateViewModel(navigatorStore, expensesStore, gymStore);

            viewModel.LoadStateCommand.Execute(null);

            return viewModel;
        }

        private ExpensesCardViewModel CreateExpenses(GymStore gymStore,AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 0;
            return ExpensesCardViewModel.LoadViewModel(gymStore, accountingStateViewModel);
        }
        private PaymentsCardViewModel CreatePayments(GymStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 1;
            return PaymentsCardViewModel.LoadViewModel(gymStore, accountingStateViewModel);
        }
        private CreditsCardViewModel CreateCredits(GymStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 2;
            return CreditsCardViewModel.LoadViewModel(gymStore, accountingStateViewModel);
        }
        private SubscriptionCardViewModel CreateSubscriptions(GymStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 3;
            return SubscriptionCardViewModel.LoadViewModel(gymStore, accountingStateViewModel);
        }
    }
}
