using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.Employee
{
    public class DeleteEmployeeCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore ;
        private readonly NavigationService<TrainersListViewModel> _navigationService;
        public DeleteEmployeeCommand(EmployeeStore employeeStore, NavigationService<TrainersListViewModel> navigationService)
        {
            _employeeStore = employeeStore;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _employeeStore.Delete(_employeeStore.SelectedEmployee!);
                _navigationService.ReNavigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
