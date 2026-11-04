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

namespace Uniceps.ViewModels.Accountant
{
    public class AccountingStateViewModel : ListingViewModelBase
    {
        private readonly DailyReportStore _dailyReportStore;

        public SubscriptionStateCardViewModel SubscriptionsViewModel { get; }
        public CreditsCardViewModel CreditsViewModel { get; }
        public PaymentsCardViewModel PaymentsViewModel { get; }
        public ExpensesCardViewModel ExpensesViewModel { get; }

        private int _selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
                LoadDataForCurrentTab(); // تحميل بيانات التبويب الحالي فقط عند الانتقال إليه!
            }
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
                LoadDataForCurrentTab();
                LoadStateCommand.Execute(null);
            }
        }

        public AccountingStateViewModel(
            DailyReportStore dailyReportStore,
            SubscriptionStateCardViewModel subscriptionsViewModel,
            CreditsCardViewModel creditsViewModel,
            PaymentsCardViewModel paymentsViewModel,
            ExpensesCardViewModel expensesViewModel)
        {
            _dailyReportStore = dailyReportStore;
            SubscriptionsViewModel = subscriptionsViewModel;
            CreditsViewModel = creditsViewModel;
            PaymentsViewModel = paymentsViewModel;
            ExpensesViewModel = expensesViewModel;

            LoadStateCommand = new LoadStatesCommand(this, _dailyReportStore);
            _dailyReportStore.PaymentSumLoaded += _dailyReportStore_PaymentSumLoaded;
            _dailyReportStore.CreditsSumLoaded += _dailyReportStore_CreditSumLoaded;
            _dailyReportStore.SubscriptionsSumLoaded += _dailyReportStore_SubscriptionsSumLoaded;
            _dailyReportStore.ExpensesSumLoaded += _dailyReportStore_ExpensesSumLoaded;
            LoadDataForCurrentTab();
            LoadStateCommand.Execute(null);
        }

        private void LoadDataForCurrentTab()
        {
            LoadStateCommand.Execute(null);
            switch (_selectedTabIndex)
            {
                case 0:
                    SubscriptionsViewModel.LoadSubscriptionsCommand.Execute(Date);
                    break;
                case 1:
                     CreditsViewModel.LoadCreditsCommand.Execute(Date);
                    break;
                case 2:
                     PaymentsViewModel.LoadPaymentsCommand.Execute(Date);
                    break;
                case 3:
                     ExpensesViewModel.LoadExpensesCommand.Execute(Date);
                    break;
            }
        }

        public ICommand LoadStateCommand { get; }

     
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
        private void _dailyReportStore_SubscriptionsSumLoaded(double obj)
        {
            SubscriptionsCard = obj;
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
     
    }
}
