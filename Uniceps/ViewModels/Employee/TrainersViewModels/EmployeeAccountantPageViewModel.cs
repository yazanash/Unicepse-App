using Uniceps.Commands.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Uniceps.Commands;
using Uniceps.ViewModels.PrintViewModels;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Core.Models.Employee;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class EmployeeAccountantPageViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly CreditViewModels.CreditListViewModel _creditListViewModel;
        ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        public CollectionViewSource GroupedTasks { get; set; }

        public IEnumerable<SubscriptionListItemViewModel> SubscriptionsList => _subscriptionListItemViewModels;
        public TrainerMounthlyReportViewModel? TrainerMounthlyReportViewModel { get; set; }
        public EmployeeAccountantPageViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, NavigationStore navigationStore, CreditsDataStore creditsDataStore, CreditViewModels.CreditListViewModel creditListViewModel)
        {
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _navigationStore = navigationStore;
            _creditsDataStore = creditsDataStore;
            _creditListViewModel = creditListViewModel;

            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _dausesDataStore.StateChanged += _dausesDataStore_StateChanged;
            GroupedTasks = new CollectionViewSource { Source = _subscriptionListItemViewModels };
            LoadMounthlyReport = new LoadTrainerMonthlyReport(_dausesDataStore, _employeeStore, this);
            PrintCommand = new PrintCommand(new PrintWindowViewModel(new TrainerDetiledReportViewModel(_employeeStore, _dausesDataStore, this), new NavigationStore()));
        }

        public ICommand? PrintCommand { get; }
        private void _dausesDataStore_StateChanged(TrainerDueses? obj)
        {
            TrainerMounthlyReportViewModel = new(obj!, _navigationStore, _employeeStore, _creditsDataStore, _creditListViewModel);
            OnPropertyChanged(nameof(TrainerMounthlyReportViewModel));
            _subscriptionListItemViewModels.Clear();
            foreach (var item in _dausesDataStore.Payments.GroupBy(x => x.Subscription))
            {
                if (item.Key != null)
                {
                    if (_subscriptionListItemViewModels.Where(x => x.Id == item.Key.Id).Any())
                        continue;
                    SubscriptionListItemViewModel subscriptionListItemViewModel = new(item.Key!);
                    _subscriptionListItemViewModels.Add(subscriptionListItemViewModel);
                    subscriptionListItemViewModel.Order = _subscriptionListItemViewModels.Where(x => x.SportName == subscriptionListItemViewModel.SportName).Count();
                }
            }
            GroupedTasks.Source = _subscriptionListItemViewModels;
            GroupedTasks.GroupDescriptions.Clear();
            GroupedTasks.GroupDescriptions.Add(new PropertyGroupDescription("SportName"));
            OnPropertyChanged(nameof(GroupedTasks));
        }

        private DateTime _reportDate = DateTime.Now;
        public DateTime ReportDate
        {
            get { return _reportDate; }
            set { _reportDate = value; OnPropertyChanged(nameof(ReportDate)); }
        }
        public ICommand LoadMounthlyReport { get; }
    }
}
