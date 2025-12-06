using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;

namespace Uniceps.Commands.Employee
{
    public class LoadTrainerMounthlyPrintedCommand : AsyncCommandBase
    {
        private readonly DausesDataStore _dausesDataStore;
        private readonly EmployeeStore _employeeStore;
        private readonly EmployeeAccountantPageViewModel _trainerDausesListViewModel;

        public LoadTrainerMounthlyPrintedCommand(DausesDataStore dausesDataStore, EmployeeStore employeeStore, EmployeeAccountantPageViewModel trainerDausesListViewModel)
        {
            _dausesDataStore = dausesDataStore;
            _trainerDausesListViewModel = trainerDausesListViewModel;
            _employeeStore = employeeStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            await _dausesDataStore.GetPrintedMonthlyReport(_employeeStore.SelectedEmployee!, _trainerDausesListViewModel.ReportDate);
        }
    }
}
