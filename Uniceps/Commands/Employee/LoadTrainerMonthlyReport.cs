
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;

namespace Uniceps.Commands.Employee
{
    public class LoadTrainerMonthlyReport : AsyncCommandBase
    {
        private readonly DausesDataStore _dausesDataStore;
        private readonly EmployeeAccountantPageViewModel _trainerDausesListViewModel;

        public LoadTrainerMonthlyReport(DausesDataStore dausesDataStore,  EmployeeAccountantPageViewModel trainerDausesListViewModel)
        {
            _dausesDataStore = dausesDataStore;
            _trainerDausesListViewModel = trainerDausesListViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                if(parameter is int trainerId)
                await _dausesDataStore.GetMonthlyReport(trainerId, _trainerDausesListViewModel.ReportDate);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
