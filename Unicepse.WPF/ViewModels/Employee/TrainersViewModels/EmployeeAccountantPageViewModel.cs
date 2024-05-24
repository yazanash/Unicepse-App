using Unicepse.Core.Models.Employee;
using Unicepse.WPF.Commands.Employee;
using Unicepse.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.WPF.utlis.common;

namespace Unicepse.WPF.ViewModels.Employee.TrainersViewModels
{
    public class EmployeeAccountantPageViewModel : ViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;

        public TrainerMounthlyReportViewModel? TrainerMounthlyReportViewModel { get; set; }
        public EmployeeAccountantPageViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore)
        {
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _dausesDataStore.StateChanged += _dausesDataStore_StateChanged;
            LoadMounthlyReport = new LoadTrainerMonthlyReport(_dausesDataStore, _employeeStore,this);
        }

        private void _dausesDataStore_StateChanged(TrainerDueses? obj)
        {
            TrainerMounthlyReportViewModel = new(obj!, _employeeStore, _dausesDataStore);
            OnPropertyChanged(nameof(TrainerMounthlyReportViewModel));
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
