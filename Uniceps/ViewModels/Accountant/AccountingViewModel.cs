using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.Expenses;
using Uniceps.ViewModels.Metrics;
using Uniceps.ViewModels.PlayersAttendenceViewModels;

namespace Uniceps.ViewModels.Accountant
{
    public class AccountingViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly ExpensesDataStore _expensesStore;
        private readonly GymStore _gymStore;
        private readonly DailyReportStore _dailyReportStore;
        private readonly PeriodReportStore _periodReportStore;
        private readonly AccountStore _accountStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public ICommand ExpensesCommand { get; }
        public ICommand IncomeReportCommand { get; }
        public ICommand ExpensesReportCommand { get; }
        public ICommand StatesReportCommand { get; }
        //public ICommand PaymentReportCommand;
        public ICommand MonthlyIncomeReportCommand { get; }


        public bool IsDaily { get; set; }
        public bool IsExpenses { get; set; }
        public bool IsExpensesReport { get; set; }
        public bool IsIncomeReport { get; set; }
        //public ICommand PaymentReportCommand;
        public bool IsIncomeFinal { get; set; }
        public AccountingViewModel(ExpensesDataStore expensesStore, GymStore gymStore, DailyReportStore dailyReportStore, PeriodReportStore periodReportStore, AccountStore accountStore)
        {
            _navigatorStore = new NavigationStore();
            _dailyReportStore = dailyReportStore;
            _expensesStore = expensesStore;
            _gymStore = gymStore;
            _accountStore = accountStore;
            _periodReportStore = periodReportStore;
            if (_accountStore.SystemSubscription != null)
            {
                _navigatorStore.CurrentViewModel = CreateStatesViewModel(_dailyReportStore);
                _navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
                ExpensesCommand = new NavaigateCommand<ExpensesListViewModel>(new NavigationService<ExpensesListViewModel>(_navigatorStore, () => CreateExpenses(_navigatorStore, _expensesStore)));
                IncomeReportCommand = new NavaigateCommand<IncomeReportViewModel>(new NavigationService<IncomeReportViewModel>(_navigatorStore, () => new IncomeReportViewModel(_periodReportStore)));
                ExpensesReportCommand = new NavaigateCommand<ExpensesReportViewModel>(new NavigationService<ExpensesReportViewModel>(_navigatorStore, () => new ExpensesReportViewModel(_periodReportStore)));
                MonthlyIncomeReportCommand = new NavaigateCommand<MounthlyReportViewModel>(new NavigationService<MounthlyReportViewModel>(_navigatorStore, () => new MounthlyReportViewModel(_gymStore)));
                StatesReportCommand = new NavaigateCommand<AccountingStateViewModel>(new NavigationService<AccountingStateViewModel>(_navigatorStore, () => CreateStatesViewModel(_dailyReportStore)));

            }
            else
            {
                _navigatorStore.CurrentViewModel = CreateExpenses(_navigatorStore, _expensesStore);
                _navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
                ExpensesCommand = new NavaigateCommand<ExpensesListViewModel>(new NavigationService<ExpensesListViewModel>(_navigatorStore, () => CreateExpenses(_navigatorStore, _expensesStore)));
                IncomeReportCommand = new NavaigateCommand<IncomeReportViewModel>(new NavigationService<IncomeReportViewModel>(_navigatorStore, () => new IncomeReportViewModel(_periodReportStore)));
                ExpensesReportCommand = new NavaigateCommand<ExpensesReportViewModel>(new NavigationService<ExpensesReportViewModel>(_navigatorStore, () => new ExpensesReportViewModel(_periodReportStore)));
                MonthlyIncomeReportCommand = new NavaigateCommand<PremiumViewModel>(new NavigationService<PremiumViewModel>(_navigatorStore, () => new PremiumViewModel()));
                StatesReportCommand = new NavaigateCommand<PremiumViewModel>(new NavigationService<PremiumViewModel>(_navigatorStore, () => new PremiumViewModel()));

            }


        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            IsDaily = CurrentViewModel is AccountingStateViewModel;
            IsExpenses = CurrentViewModel is ExpensesListViewModel;
            IsExpensesReport = CurrentViewModel is ExpensesReportViewModel;
            IsIncomeReport = CurrentViewModel is IncomeReportViewModel;
            IsIncomeFinal = CurrentViewModel is MounthlyReportViewModel;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        public void ResetSelection()
        {
            if (_accountStore.SystemSubscription != null)
            {
                _navigatorStore.CurrentViewModel = CreateStatesViewModel(_dailyReportStore);
            }
            else
            {
                _navigatorStore.CurrentViewModel = CreateExpenses(_navigatorStore, _expensesStore);
            }
        }
        private AccountingStateViewModel CreateStatesViewModel(DailyReportStore dailyReportStore)
        {
            NavigationStore navigatorStore = new NavigationStore();
            return AccountingStateViewModel.LoadViewModel(navigatorStore, dailyReportStore);
        }
        private ExpensesListViewModel CreateExpenses(NavigationStore navigatorStore, ExpensesDataStore expensesStore)
        {
            return ExpensesListViewModel.LoadViewModel(navigatorStore, expensesStore);
        }

    }
}
