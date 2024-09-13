using Unicepse.Commands.Employee;
using Unicepse.Stores;
using Unicepse.ViewModels.Employee.TrainersViewModels;
using Unicepse.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.utlis.common;
using Unicepse.Core.Models.Employee;
using System.Windows.Data;

namespace Unicepse.ViewModels.PrintViewModels
{
    public class TrainerDetiledReportViewModel : ViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        public CollectionViewSource GroupedTasks { get; set; }
        public IEnumerable<SubscriptionListItemViewModel> Subscriptions => _subscriptionListItemViewModels;
        public EmployeeAccountantPageViewModel _employeeAccountantPageViewModel;
        public PrintedTrainerMounthlyReportViewModel? TrainerMounthlyReportViewModel { get; set; }

        public TrainerDetiledReportViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, EmployeeAccountantPageViewModel employeeAccountantPageViewModel)
        {
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _dausesDataStore.StateChanged += _dausesDataStore_StateChanged;
            _employeeAccountantPageViewModel = employeeAccountantPageViewModel;
            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            GroupedTasks = new CollectionViewSource(); 
            ReportDate = _employeeAccountantPageViewModel.ReportDate.ToShortDateString();
            FullName = _employeeStore.SelectedEmployee!.FullName;
        }

        private void _dausesDataStore_StateChanged(TrainerDueses? obj)
        {
            TrainerMounthlyReportViewModel = new(obj!);
            _subscriptionListItemViewModels.Clear();
            foreach (var item in _employeeAccountantPageViewModel.SubscriptionsList)
            {
                _subscriptionListItemViewModels.Add(item);
            }
            GroupedTasks.Source = _subscriptionListItemViewModels;
            GroupedTasks.GroupDescriptions.Add(new PropertyGroupDescription("SportName"));
        }

        private string? _fullName;
        public string? FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }
        private string? _reportDate;
        public string? ReportDate
        {
            get { return _reportDate; }
            set { _reportDate = value; OnPropertyChanged(nameof(ReportDate)); }
        }
        
    }
}
