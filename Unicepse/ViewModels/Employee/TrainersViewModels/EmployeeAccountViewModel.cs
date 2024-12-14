using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.ViewModels.Employee.CreditViewModels;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;
using Unicepse.Stores.EmployeeStores;

namespace Unicepse.ViewModels.Employee.TrainersViewModels
{
    public class EmployeeAccountViewModel : ViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly LicenseDataStore _licenseDataStore;
        private readonly EmployeeSubscriptionDataStore _employeeSubscriptionDataStore;
        public TrainerListItemViewModel? Employee { get; set; }
        public ViewModelBase? CurrentEmployeeViewModel => _navigatorStore.CurrentViewModel;

        public EmployeeAccountViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, TrainerListItemViewModel? employee, LicenseDataStore licenseDataStore, EmployeeSubscriptionDataStore employeeSubscriptionDataStore)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            _licenseDataStore = licenseDataStore;
            Employee = employee;
            _employeeSubscriptionDataStore = employeeSubscriptionDataStore;

            IsTrainer = _employeeStore.SelectedEmployee!.IsTrainer;
            if (_employeeStore.SelectedEmployee!.IsTrainer)
                navigatorStore.CurrentViewModel = LoadEmployeeAccountantPageViewModel(_employeeStore, _dausesDataStore, _navigatorStore, _creditsDataStore, LoadEmployeeCredit(_navigatorStore, _employeeStore, _creditsDataStore), _licenseDataStore);
            else
                navigatorStore.CurrentViewModel = LoadEmployeeCredit(_navigatorStore, _employeeStore, _creditsDataStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            EmployeeCreditsCommand = new NavaigateCommand<CreditListViewModel>(new NavigationService<CreditListViewModel>(_navigatorStore, () => LoadEmployeeCredit(_navigatorStore, _employeeStore, _creditsDataStore)));
            TrainerPlayersCommand = new NavaigateCommand<TrainerSubscriptionViewModel>(new NavigationService<TrainerSubscriptionViewModel>(_navigatorStore, () => LoadTrainerSubscriptions(_employeeStore, _employeeSubscriptionDataStore)));
            TrainerDusesCommand = new NavaigateCommand<EmployeeAccountantPageViewModel>(new NavigationService<EmployeeAccountantPageViewModel>(_navigatorStore, () => LoadEmployeeAccountantPageViewModel(_employeeStore, _dausesDataStore, _navigatorStore, _creditsDataStore, LoadEmployeeCredit(_navigatorStore, _employeeStore, _creditsDataStore), _licenseDataStore)));

            //PaymentCommand = new NavaigateCommand<PaymentListViewModel>(new NavigationService<PaymentListViewModel>(_navigatorStore, () => LoadPaymentsViewModel(_paymentDataStore, _playersDataStore, _navigatorStore, _subscriptionStore)));
            //MetricsCommand = new NavaigateCommand<MetricReportViewModel>(new NavigationService<MetricReportViewModel>(_navigatorStore, () => LoadMetricsViewModel(_metricDataStore, _playersDataStore, _navigatorStore)));
            //TrainingProgramCommand = new NavaigateCommand<RoutinePlayerViewModels>(new NavigationService<RoutinePlayerViewModels>(_navigatorStore, () => LoadRoutineViewModel(_routineDataStore, _playersDataStore, _navigatorStore)));
        }
        private bool _isTrainer;
        public bool IsTrainer
        {
            get { return _isTrainer; }
            set { _isTrainer = value; OnPropertyChanged(nameof(IsTrainer)); }
        }

        private CreditListViewModel LoadEmployeeCredit(NavigationStore navigatorStore, EmployeeStore employeeStore, CreditsDataStore creditsDataStore)
        {
            return CreditListViewModel.LoadViewModel(employeeStore, creditsDataStore, navigatorStore);
        }
        private TrainerSubscriptionViewModel LoadTrainerSubscriptions(EmployeeStore employeeStore, EmployeeSubscriptionDataStore subscriptionDataStore)
        {
            return TrainerSubscriptionViewModel.LoadViewModel(employeeStore, subscriptionDataStore);
        }
        private EmployeeAccountantPageViewModel LoadEmployeeAccountantPageViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, NavigationStore navigationStore
            ,CreditsDataStore creditsDataStore, CreditListViewModel creditListViewModel,LicenseDataStore licenseDataStore)
        {
            return new EmployeeAccountantPageViewModel(employeeStore, dausesDataStore, navigationStore, creditsDataStore, creditListViewModel,licenseDataStore);
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentEmployeeViewModel));
        }


        public ICommand? EmployeeCreditsCommand { get; }
        public ICommand? TrainerPlayersCommand { get; }
        public ICommand? TrainerDusesCommand { get; }
        public ICommand? MetricsCommand { get; }
        public ICommand? TrainingProgramCommand { get; }
    }
}
