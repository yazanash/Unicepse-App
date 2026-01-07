using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.Stores.EmployeeStores;
using Uniceps.ViewModels.Accountant;
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.ViewModels.Expenses;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class EmployeeAccountViewModel : ViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly EmployeeSubscriptionDataStore _employeeSubscriptionDataStore;
        private readonly AccountStore _accountStore;
        public TrainerListItemViewModel? Employee { get; set; }
        public ViewModelBase? CurrentEmployeeViewModel => _navigatorStore.CurrentViewModel;

        public EmployeeAccountViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, TrainerListItemViewModel? employee, EmployeeSubscriptionDataStore employeeSubscriptionDataStore, AccountStore accountStore)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            Employee = employee;
            _employeeSubscriptionDataStore = employeeSubscriptionDataStore;
            _accountStore = accountStore;

            IsTrainer = _employeeStore.SelectedEmployee!.IsTrainer;
            if (_accountStore.SystemSubscription != null)
            {
                if (_employeeStore.SelectedEmployee!.IsTrainer)
                    navigatorStore.CurrentViewModel = LoadEmployeeAccountantPageViewModel(_employeeStore, _dausesDataStore, _navigatorStore, _creditsDataStore, LoadEmployeeCredit(_navigatorStore, _employeeStore, _creditsDataStore));
                else
                    navigatorStore.CurrentViewModel = LoadEmployeeCredit(_navigatorStore, _employeeStore, _creditsDataStore);
                navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
                EmployeeCreditsCommand = new NavaigateCommand<CreditListViewModel>(new NavigationService<CreditListViewModel>(_navigatorStore, () => LoadEmployeeCredit(_navigatorStore, _employeeStore, _creditsDataStore)));
                TrainerPlayersCommand = new NavaigateCommand<TrainerSubscriptionViewModel>(new NavigationService<TrainerSubscriptionViewModel>(_navigatorStore, () => LoadTrainerSubscriptions(_employeeStore, _employeeSubscriptionDataStore)));
                TrainerDusesCommand = new NavaigateCommand<EmployeeAccountantPageViewModel>(new NavigationService<EmployeeAccountantPageViewModel>(_navigatorStore, () => LoadEmployeeAccountantPageViewModel(_employeeStore, _dausesDataStore, _navigatorStore, _creditsDataStore, LoadEmployeeCredit(_navigatorStore, _employeeStore, _creditsDataStore))));
            }
            else
            {
                navigatorStore.CurrentViewModel = LoadEmployeeCredit(_navigatorStore, _employeeStore, _creditsDataStore);
                navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
                EmployeeCreditsCommand = new NavaigateCommand<CreditListViewModel>(new NavigationService<CreditListViewModel>(_navigatorStore, () => LoadEmployeeCredit(_navigatorStore, _employeeStore, _creditsDataStore)));
                TrainerPlayersCommand = new NavaigateCommand<PremiumViewModel>(new NavigationService<PremiumViewModel>(_navigatorStore, () =>new PremiumViewModel()));
                TrainerDusesCommand = new NavaigateCommand<PremiumViewModel>(new NavigationService<PremiumViewModel>(_navigatorStore, () => new PremiumViewModel()));
            }

        }
        private bool _isTrainer;
        public bool IsTrainer
        {
            get { return _isTrainer; }
            set { _isTrainer = value; OnPropertyChanged(nameof(IsTrainer)); }
        }
        public bool IsCredit { get; set; }
        public bool IsPlayers { get; set; }
        private CreditListViewModel LoadEmployeeCredit(NavigationStore navigatorStore, EmployeeStore employeeStore, CreditsDataStore creditsDataStore)
        {
            return CreditListViewModel.LoadViewModel(employeeStore, creditsDataStore, navigatorStore);
        }
        private TrainerSubscriptionViewModel LoadTrainerSubscriptions(EmployeeStore employeeStore, EmployeeSubscriptionDataStore subscriptionDataStore)
        {
            return TrainerSubscriptionViewModel.LoadViewModel(employeeStore, subscriptionDataStore);
        }
        private EmployeeAccountantPageViewModel LoadEmployeeAccountantPageViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, NavigationStore navigationStore
            , CreditsDataStore creditsDataStore, CreditListViewModel creditListViewModel)
        {
            return new EmployeeAccountantPageViewModel(employeeStore, dausesDataStore, navigationStore, creditsDataStore, creditListViewModel);
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            IsTrainer = CurrentEmployeeViewModel is EmployeeAccountantPageViewModel;
            IsCredit = CurrentEmployeeViewModel is TrainerSubscriptionViewModel;
            IsPlayers = CurrentEmployeeViewModel is PlayerMainPageViewModel;
            OnPropertyChanged(nameof(CurrentEmployeeViewModel));
        }


        public ICommand? EmployeeCreditsCommand { get; }
        public ICommand? TrainerPlayersCommand { get; }
        public ICommand? TrainerDusesCommand { get; }
        public ICommand? MetricsCommand { get; }
        public ICommand? TrainingProgramCommand { get; }
    }
}
