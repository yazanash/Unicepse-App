using PlatinumGymPro.Commands.Employee;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Employee.TrainersViewModels;
using PlatinumGymPro.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PrintViewModels
{
    public class TrainerDetiledReportViewModel :ViewModelBase
    {
        //private readonly EmployeeStore _employeeStore;
        //private readonly DausesDataStore  _dausesDataStore;
        //private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        //public IEnumerable<SubscriptionListItemViewModel> Subscriptions => _subscriptionListItemViewModels;
        //public EmployeeAccountantPageViewModel _employeeAccountantPageViewModel;
        //public TrainerMounthlyReportViewModel? TrainerMounthlyReportViewModel { get; set; }
        //public TrainerDetiledReportViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, EmployeeAccountantPageViewModel employeeAccountantPageViewModel)
        //{
        //    _employeeStore = employeeStore;
        //    _dausesDataStore = dausesDataStore;
        //    //_dausesDataStore.StateChanged += _dausesDataStore_StateChanged;
        //    _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
        //    _employeeAccountantPageViewModel = employeeAccountantPageViewModel;
        //    FullName = _employeeStore.SelectedEmployee!.FullName;
        //    ReportDate = _employeeAccountantPageViewModel.ReportDate;
        //    TrainerMounthlyReportViewModel = employeeAccountantPageViewModel.TrainerMounthlyReportViewModel;
        //    foreach (var item in _dausesDataStore._payments.GroupBy(x => x.Subscription))
        //    {
        //        if (_subscriptionListItemViewModels.Where(x => x.Subscription.Id == item.Key!.Id).Count() > 0)
        //        {
        //            SubscriptionListItemViewModel subscriptionListItemViewModel = new SubscriptionListItemViewModel(item.Key!);
        //            _subscriptionListItemViewModels.Add(subscriptionListItemViewModel);
        //        }

        //    }
        //    //LoadMounthlyReport = new LoadTrainerMonthlyReport(_dausesDataStore, _employeeStore, _employeeAccountantPageViewModel);
        //}

        ////private void _dausesDataStore_StateChanged(PlatinumGym.Core.Models.Employee.TrainerDueses? obj)
        ////{
        ////    TrainerMounthlyReportViewModel = new(obj!, _employeeStore, _dausesDataStore);
        ////    OnPropertyChanged(nameof(TrainerMounthlyReportViewModel));
        ////    _subscriptionListItemViewModels.Clear();
        ////    foreach(var item in _dausesDataStore._payments.GroupBy(x=>x.Subscription))
        ////    {
        ////        if (_subscriptionListItemViewModels.Where(x => x.Subscription.Id == item.Key!.Id).Count() > 0)
        ////        {
        ////            SubscriptionListItemViewModel subscriptionListItemViewModel = new SubscriptionListItemViewModel(item.Key!);
        ////            _subscriptionListItemViewModels.Add(subscriptionListItemViewModel);
        ////        }

        ////    }
        ////}

        ////public ICommand LoadMounthlyReport { get; }
        //private string? _fullName;
        //public string? FullName
        //{
        //    get { return _fullName; }
        //    set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        //}
        //private DateTime _reportDate;
        //public DateTime ReportDate
        //{
        //    get { return _reportDate; }
        //    set { _reportDate = value; OnPropertyChanged(nameof(ReportDate)); }
        //}
        //public static TrainerDetiledReportViewModel LoadViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, EmployeeAccountantPageViewModel employeeAccountantPageViewModel)
        //{
        //    TrainerDetiledReportViewModel viewModel = new TrainerDetiledReportViewModel(employeeStore, dausesDataStore, employeeAccountantPageViewModel);
        //    //viewModel.LoadMounthlyReport.Execute(null);
        //    return viewModel;
        //}
    }
}
