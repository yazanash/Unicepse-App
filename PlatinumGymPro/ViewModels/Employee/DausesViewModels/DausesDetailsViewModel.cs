using PlatinumGym.Core.Models.Employee;
using PlatinumGymPro.Commands.Employee;
using PlatinumGymPro.Commands.Employee.DauseCommads;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Employee.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.Employee.DausesViewModels
{
    public class DausesDetailsViewModel : ErrorNotifyViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly NavigationStore _navigatorStore;
        private readonly DauseListViewModel _dauseListViewModel;
        public TrainerMounthlyReportViewModel? TrainerMounthlyReportViewModel { get; set; }
        public DausesDetailsViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, NavigationStore navigatorStore, DauseListViewModel dauseListViewModel)
        {
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _navigatorStore = navigatorStore;
            _dauseListViewModel=dauseListViewModel;
            _dausesDataStore.StateChanged += _dausesDataStore_StateChanged;
            //LoadMounthlyReport = new LoadTrainerMonthlyReport(_dausesDataStore, _employeeStore, this);
            TrainerMounthlyReportViewModel = new TrainerMounthlyReportViewModel(new TrainerDueses(),_employeeStore,_dausesDataStore);
            SaveMounthlyReport = new SubmitDauseCommand(_employeeStore, _dausesDataStore, this,new NavigationService<DauseListViewModel>(_navigatorStore,()=>_dauseListViewModel));
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
            set { _reportDate = value; 
                OnPropertyChanged(nameof(ReportDate)); 
                
            }
        }
        public ICommand LoadMounthlyReport { get; }
        public ICommand SaveMounthlyReport { get; }
    }
}
