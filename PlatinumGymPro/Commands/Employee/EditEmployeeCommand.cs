using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Employee.TrainersViewModels;
using PlatinumGymPro.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emp = PlatinumGym.Core.Models.Employee;
namespace PlatinumGymPro.Commands.Employee
{
    public class EditEmployeeCommand : AsyncCommandBase
    {
        private readonly NavigationService<TrainersListViewModel> navigationService;
        private readonly EmployeeStore _employeeStore;
        private EditEmployeeViewModel _editEmployeeViewModel;

        public EditEmployeeCommand(NavigationService<TrainersListViewModel> navigationService, EditEmployeeViewModel editEmployeeViewModel, EmployeeStore employeeStore)
        {
            this.navigationService = navigationService;
            _employeeStore = employeeStore;
            _editEmployeeViewModel = editEmployeeViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            emp.Employee employee = new emp.Employee()
            {
                Id = _employeeStore.SelectedEmployee!.Id,
                FullName = _editEmployeeViewModel.FullName,
                BirthDate = _editEmployeeViewModel.BirthDate,
                GenderMale = _editEmployeeViewModel.GenderMale,
                IsActive = true,
                IsTrainer = false,
                IsSecrtaria = _editEmployeeViewModel.IsSecertary,
                ParcentValue = _editEmployeeViewModel.ParcentValue,
                SalaryValue = _editEmployeeViewModel.SalaryValue,
                Phone = _editEmployeeViewModel.Phone,
                StartDate = _editEmployeeViewModel.StartDate,
                Position = _editEmployeeViewModel.Position,
            };
          
            await _employeeStore.Update(employee);
            navigationService.ReNavigate();
        }

    }
}
