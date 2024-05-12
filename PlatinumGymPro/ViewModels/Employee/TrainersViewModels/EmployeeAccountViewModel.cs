using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Employee.CreditViewModels;
using PlatinumGymPro.ViewModels.Employee.DausesViewModels;
using PlatinumGymPro.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.Employee.TrainersViewModels
{
    public class EmployeeAccountViewModel : ViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly EmployeeStore  _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly CreditsDataStore _creditsDataStore;
        public TrainerListItemViewModel? Employee => new(_employeeStore.SelectedEmployee!) ;
        public ViewModelBase? CurrentEmployeeViewModel => _navigatorStore.CurrentViewModel;

        public EmployeeAccountViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            IsTrainer = _employeeStore.SelectedEmployee!.IsTrainer;
            if(_employeeStore.SelectedEmployee!.IsTrainer)
            navigatorStore.CurrentViewModel = LoadEmployeeAccountantPageViewModel(_employeeStore, _dausesDataStore,_navigatorStore);
            else
                navigatorStore.CurrentViewModel = LoadEmployeeCredit(_navigatorStore,_employeeStore, _creditsDataStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            EmployeeCreditsCommand = new NavaigateCommand<CreditListViewModel>(new NavigationService<CreditListViewModel>(_navigatorStore, () => LoadEmployeeCredit(_navigatorStore, _employeeStore,_creditsDataStore)));
            //SubscriptionCommand = new NavaigateCommand<SubscriptionDetailsViewModel>(new NavigationService<SubscriptionDetailsViewModel>(_navigatorStore, () => LoadSubscriptionViewModel(_navigatorStore, _sportDataStore, _subscriptionStore, _playersDataStore, _paymentDataStore)));
            //PaymentCommand = new NavaigateCommand<PaymentListViewModel>(new NavigationService<PaymentListViewModel>(_navigatorStore, () => LoadPaymentsViewModel(_paymentDataStore, _playersDataStore, _navigatorStore, _subscriptionStore)));
            //MetricsCommand = new NavaigateCommand<MetricReportViewModel>(new NavigationService<MetricReportViewModel>(_navigatorStore, () => LoadMetricsViewModel(_metricDataStore, _playersDataStore, _navigatorStore)));
            //TrainingProgramCommand = new NavaigateCommand<RoutinePlayerViewModels>(new NavigationService<RoutinePlayerViewModels>(_navigatorStore, () => LoadRoutineViewModel(_routineDataStore, _playersDataStore, _navigatorStore)));
        }
        private bool _isTrainer;
        public bool IsTrainer
        {
            get { return _isTrainer; }
            set { _isTrainer = value;  OnPropertyChanged(nameof(IsTrainer)); }
        }
        private CreditListViewModel LoadEmployeeCredit(NavigationStore navigatorStore, EmployeeStore employeeStore, CreditsDataStore creditsDataStore)
        {
           return CreditListViewModel.LoadViewModel(employeeStore, creditsDataStore, navigatorStore);
        }

        private DauseListViewModel LoadEmployeeAccountantPageViewModel( EmployeeStore employeeStore,DausesDataStore dausesDataStore,NavigationStore navigationStore)
        {
            return DauseListViewModel.LoadViewModel(employeeStore, dausesDataStore, navigationStore);
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentEmployeeViewModel));
        }
       
       
        public ICommand? EmployeeCreditsCommand { get; }
        public ICommand? SubscriptionCommand { get; }
        public ICommand? PaymentCommand { get; }
        public ICommand? MetricsCommand { get; }
        public ICommand? TrainingProgramCommand { get; }
    }
}
