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
    public class AddEmployeeCommand : AsyncCommandBase
    {
        private readonly NavigationService<TrainersListViewModel> navigationService;
        private readonly EmployeeStore _employeeStore;
        private AddEmployeeViewModel _addEmployeeViewModel;

        public AddEmployeeCommand(NavigationService<TrainersListViewModel> navigationService, AddEmployeeViewModel addEmployeeViewModel, EmployeeStore employeeStore)
        {
            this.navigationService = navigationService;
            _employeeStore = employeeStore;
            _addEmployeeViewModel = addEmployeeViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            emp.Employee employee = new emp.Employee()
            {
                FullName = _addEmployeeViewModel.FullName,
                Balance = _addEmployeeViewModel.Balance,
                BirthDate = _addEmployeeViewModel.BirthDate,
                GenderMale = _addEmployeeViewModel.GenderMale,
                IsActive = true,
                IsTrainer = false,
                IsSecrtaria = _addEmployeeViewModel.IsSecertary,

                ParcentValue = _addEmployeeViewModel.ParcentValue,
                SalaryValue = _addEmployeeViewModel.SalaryValue,
                Phone = _addEmployeeViewModel.Phone,
                StartDate = _addEmployeeViewModel.StartDate,
                Position = _addEmployeeViewModel.Position,
            };
         
            await _employeeStore.Add(employee);
            navigationService.ReNavigate();
        }

    }
}
