﻿using Unicepse.Core.Models.Employee;
using Unicepse.Commands.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Stores;
using Unicepse.utlis.common;
using System.Collections.ObjectModel;
using Unicepse.ViewModels.SubscriptionViewModel;
using System.Windows.Data;
using Unicepse.Commands;
using Unicepse.ViewModels.PrintViewModels;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Employee.TrainersViewModels
{
    public class EmployeeAccountantPageViewModel : ViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        public CollectionViewSource GroupedTasks { get; set; }

        public IEnumerable<SubscriptionListItemViewModel> SubscriptionsList => _subscriptionListItemViewModels;
        public TrainerMounthlyReportViewModel? TrainerMounthlyReportViewModel { get; set; }
        public EmployeeAccountantPageViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore)
        {
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _dausesDataStore.StateChanged += _dausesDataStore_StateChanged;
            GroupedTasks = new CollectionViewSource { Source = _subscriptionListItemViewModels };
            LoadMounthlyReport = new LoadTrainerMonthlyReport(_dausesDataStore, _employeeStore, this);
            PrintCommand = new PrintCommand(new PrintWindowViewModel(new TrainerDetiledReportViewModel(_employeeStore, _dausesDataStore, this), new NavigationStore()));
        }

        public ICommand? PrintCommand { get; }
        private void _dausesDataStore_StateChanged(TrainerDueses? obj)
        {
            TrainerMounthlyReportViewModel = new(obj!);
            OnPropertyChanged(nameof(TrainerMounthlyReportViewModel));
            _subscriptionListItemViewModels.Clear();
            foreach (var item in _dausesDataStore.Payments.GroupBy(x => x.Subscription))
            {
                if (item.Key != null)
                {
                    SubscriptionListItemViewModel subscriptionListItemViewModel = new(item.Key!);
                    _subscriptionListItemViewModels.Add(subscriptionListItemViewModel);
                    subscriptionListItemViewModel.Order = _subscriptionListItemViewModels.Where(x=>x.SportName==subscriptionListItemViewModel.SportName).Count();
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
