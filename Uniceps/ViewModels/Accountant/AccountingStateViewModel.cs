using Uniceps.Commands.AccountantCommand;
using Uniceps.Commands.ExpensesCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Exp = Uniceps.Core.Models.Expenses;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.navigation.Stores;

namespace Uniceps.ViewModels.Accountant
{
    public class AccountingStateViewModel : ListingViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly DailyReportStore _dailyReportStore;

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
        public AccountingStateViewModel(NavigationStore navigationStore, DailyReportStore dailyReportStore)
        {
            _navigationStore = navigationStore;
            _dailyReportStore = dailyReportStore;

            LoadStateCommand = new LoadStatesCommand(this, _dailyReportStore);
            _dailyReportStore.PaymentSumLoaded += _dailyReportStore_PaymentSumLoaded;
            _dailyReportStore.CreditsSumLoaded += _dailyReportStore_CreditSumLoaded;
            _dailyReportStore.SubscriptionsSumLoaded += _dailyReportStore_SubscriptionsSumLoaded;
            _dailyReportStore.ExpensesSumLoaded += _dailyReportStore_ExpensesSumLoaded;
            _navigationStore.CurrentViewModel = CreateSubscriptions(_dailyReportStore, this);
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
            ExpensesCommand = new NavaigateCommand<ExpensesCardViewModel>(new NavigationService<ExpensesCardViewModel>(_navigationStore, () => CreateExpenses(_dailyReportStore, this)));
            IncomeCommand = new NavaigateCommand<PaymentsCardViewModel>(new NavigationService<PaymentsCardViewModel>(_navigationStore, () => CreatePayments(_dailyReportStore, this)));
            CreditsCommand = new NavaigateCommand<CreditsCardViewModel>(new NavigationService<CreditsCardViewModel>(_navigationStore, () => CreateCredits(_dailyReportStore, this)));
            SubscriptionsCommand = new NavaigateCommand<SubscriptionCardViewModel>(new NavigationService<SubscriptionCardViewModel>(_navigationStore, () => CreateSubscriptions(_dailyReportStore, this)));
        }

        private void _dailyReportStore_SubscriptionsSumLoaded(double obj)
        {
            SubscriptionsCard = obj;
        }

        private void _navigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void _dailyReportStore_CreditSumLoaded(double obj)
        {
            CreditsCard = obj;
        }

        private void _dailyReportStore_ExpensesSumLoaded(double obj)
        {
            ExpensesCard = obj;
        }

        private void _dailyReportStore_PaymentSumLoaded(double obj)
        {
            PaymentsCard = obj;
        }
        public ICommand LoadStateCommand;
        public static AccountingStateViewModel LoadViewModel(NavigationStore navigatorStore, DailyReportStore dailyReportStore)
        {
            AccountingStateViewModel viewModel = new AccountingStateViewModel(navigatorStore, dailyReportStore);

            viewModel.LoadStateCommand.Execute(null);

            return viewModel;
        }

        private ExpensesCardViewModel CreateExpenses(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 0;
            return ExpensesCardViewModel.LoadViewModel(dailyReportStore, accountingStateViewModel);
        }
        private PaymentsCardViewModel CreatePayments(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 1;
            return PaymentsCardViewModel.LoadViewModel(dailyReportStore, accountingStateViewModel);
        }
        private CreditsCardViewModel CreateCredits(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 2;
            return CreditsCardViewModel.LoadViewModel(dailyReportStore, accountingStateViewModel);
        }
        private SubscriptionCardViewModel CreateSubscriptions(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            ViewNum = 3;
            return SubscriptionCardViewModel.LoadViewModel(dailyReportStore, accountingStateViewModel);
        }
    }
}
