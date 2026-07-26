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
using Emp = Uniceps.Core.Models.Employee;
namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class EmployeeAccountantPageViewModel : ViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly DausesDataStore _dausesDataStore;
        ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        public CollectionViewSource GroupedTasks { get; set; }

        public IEnumerable<SubscriptionListItemViewModel> SubscriptionsList => _subscriptionListItemViewModels;
        public TrainerMounthlyReportViewModel? TrainerMounthlyReportViewModel { get; set; }
        public Emp.Employee SelectedEmployee { get; set; }
        public EmployeeAccountantPageViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, Emp.Employee selectedEmployee)
        {
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            SelectedEmployee = selectedEmployee;

            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _dausesDataStore.StateChanged += _dausesDataStore_StateChanged;
            _dausesDataStore.Closed += _dausesDataStore_Closed;
            GroupedTasks = new CollectionViewSource { Source = _subscriptionListItemViewModels };
            LoadMounthlyReport = new LoadTrainerMonthlyReport(_dausesDataStore, this);
        }

        private void _dausesDataStore_Closed(bool obj)
        {
            if (obj) MessageBox.Show("تم ترصيد الحساب بنجاح");
            else MessageBox.Show("فشل ترصيد الحساب");
        }

        public ICommand PrintCommand => new AsyncRelayCommand(ExecuteExportToExcelCommand);
        private async Task  ExecuteExportToExcelCommand()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "احفظ الملف",
                FileName = SelectedEmployee?.FullName + DateTime.Now.ToString("dd-MM-yyyy _ HH-mm") + ".xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                var filePath = dialog.FileName;
                if (string.IsNullOrWhiteSpace(filePath)) return;
                try
                {
                    await _dausesDataStore.ExportMonthlyReport(SelectedEmployee!.Id, filePath, ReportDate);
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
            TrainerMounthlyReportViewModel = new(obj!, _employeeStore, _creditsDataStore,_dausesDataStore);
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
