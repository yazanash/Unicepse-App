using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Employee;
using Uniceps.Core.Models.Employee;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.PrintViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

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

        }

        public ICommand PrintCommand => new AsyncRelayCommand(ExecuteExportToExcelCommand);
        private async Task  ExecuteExportToExcelCommand()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "احفظ الملف",
                FileName = _employeeStore.SelectedEmployee?.FullName + DateTime.Now.ToString("dd-MM-yyyy _ HH-mm") + ".xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                var filePath = dialog.FileName;
                if (string.IsNullOrWhiteSpace(filePath)) return;
                try
                {
                    await _dausesDataStore.ExportMonthlyReport(_employeeStore.SelectedEmployee!, ReportDate, filePath);
                    MessageBox.Show("تم التصدير بنجاح");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("فشلت عملية التصدير :"+ex.Message);
                }
               
            }
            else
            {
                MessageBox.Show("تم الغاء العملية من قبل المستخدم");
            }
        }

        private void _dausesDataStore_StateChanged(TrainerDueses? obj)
        {
            TrainerMounthlyReportViewModel = new(obj!, _navigationStore, _employeeStore, _creditsDataStore, _creditListViewModel);
            OnPropertyChanged(nameof(TrainerMounthlyReportViewModel));
            _subscriptionListItemViewModels.Clear();
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
