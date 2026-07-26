using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Emp = Uniceps.Core.Models.Employee;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class EmployeeAccountViewModel : ViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly LicenseStore _licenseStore;
        public TrainerListItemViewModel? Employee { get; set; }
        public ViewModelBase? CurrentEmployeeViewModel => _navigatorStore.CurrentViewModel;
        public Emp.Employee SelectedEmployee;
        public EmployeeAccountViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, TrainerListItemViewModel? employee, LicenseStore licenseStore, Emp.Employee selectedEmployee)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            Employee = employee;
            _licenseStore = licenseStore;
            SelectedEmployee = selectedEmployee;

            IsTrainer = SelectedEmployee!.IsTrainer;
            if (_licenseStore.Current.IsFullVersion)
            {
                if (SelectedEmployee!.IsTrainer)
                    navigatorStore.CurrentViewModel = new EmployeeAccountantPageViewModel(_employeeStore, _dausesDataStore, _creditsDataStore, SelectedEmployee);
                else
                    navigatorStore.CurrentViewModel = new CreditListViewModel(_creditsDataStore, _navigatorStore, SelectedEmployee);
                navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
                EmployeeCreditsCommand = new NavaigateCommand<CreditListViewModel>(new NavigationService<CreditListViewModel>(_navigatorStore, () => new CreditListViewModel(_creditsDataStore,_navigatorStore, SelectedEmployee)));
               TrainerDusesCommand = new NavaigateCommand<EmployeeAccountantPageViewModel>(new NavigationService<EmployeeAccountantPageViewModel>(_navigatorStore, () => new EmployeeAccountantPageViewModel(employeeStore, dausesDataStore, creditsDataStore, SelectedEmployee)));
            }
            else
            {
                navigatorStore.CurrentViewModel = new CreditListViewModel(_creditsDataStore, _navigatorStore, SelectedEmployee);
                navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
                EmployeeCreditsCommand = new NavaigateCommand<CreditListViewModel>(new NavigationService<CreditListViewModel>(_navigatorStore, () => new CreditListViewModel(_creditsDataStore, _navigatorStore, SelectedEmployee)));
                TrainerDusesCommand = new NavaigateCommand<PremiumViewModel>(new NavigationService<PremiumViewModel>(_navigatorStore, () => new PremiumViewModel()));
            }

        }
        private bool _isTrainer;
        public bool IsTrainer
        {
            get { return _isTrainer; }
            set { _isTrainer = value; OnPropertyChanged(nameof(IsTrainer)); }
        }
        private bool _isTrainerPage;
        public bool IsTrainerPage
        {
            get { return _isTrainerPage; }
            set { _isTrainerPage = value; OnPropertyChanged(nameof(IsTrainerPage)); }
        }
        public bool IsCredit { get; set; }
        public bool IsPlayers { get; set; }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            IsTrainerPage = CurrentEmployeeViewModel is EmployeeAccountantPageViewModel;
            IsPlayers = CurrentEmployeeViewModel is PlayerMainPageViewModel;
            OnPropertyChanged(nameof(CurrentEmployeeViewModel));
        }


        public ICommand? EmployeeCreditsCommand { get; }
        public ICommand? TrainerDusesCommand { get; }
    }
}
