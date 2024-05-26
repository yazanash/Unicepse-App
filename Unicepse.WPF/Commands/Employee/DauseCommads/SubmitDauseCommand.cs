using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Employee;
using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.Employee.DausesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.WPF.navigation;

namespace Unicepse.WPF.Commands.Employee.DauseCommads
{
    public class SubmitDauseCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        private DausesDetailsViewModel _dausesDetailsViewModel;
        private NavigationService<DauseListViewModel> _navigationService;
        public SubmitDauseCommand(EmployeeStore employeeStore, DausesDataStore dausesDataStore, DausesDetailsViewModel dausesDetailsViewModel, NavigationService<DauseListViewModel> navigationService)
        {
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _dausesDetailsViewModel = dausesDetailsViewModel;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                TrainerDueses trainerDues = _dausesDetailsViewModel.TrainerMounthlyReportViewModel!.trainerDueses;
                trainerDues.Trainer = _employeeStore.SelectedEmployee;
                var td = _dausesDataStore._dauses.Where(x => x.IssueDate.Month == trainerDues.IssueDate.Month && x.IssueDate.Year == trainerDues.IssueDate.Year).SingleOrDefault();
                if (td != null)
                {
                    if (trainerDues.IssueDate > td.IssueDate)
                    {
                        trainerDues.Id = td.Id;
                        await _dausesDataStore.Update(trainerDues);
                    }
                    else
                    {
                        throw new InvalidDateExeption("خطأ في التاريخ , التاريخ المحدد اقل من تاريخ اخر تقرير");
                    }
                }
                else
                    await _dausesDataStore.Add(trainerDues);

                _navigationService.ReNavigate();
            }
            catch(Exception ex)
            {
                _dausesDetailsViewModel.ClearError(nameof(_dausesDetailsViewModel.ReportDate));
                _dausesDetailsViewModel.AddError(ex.Message, nameof(_dausesDetailsViewModel.ReportDate));
                _dausesDetailsViewModel.OnErrorChanged(nameof(_dausesDetailsViewModel.ReportDate));
            }
           
        }
    }
}
