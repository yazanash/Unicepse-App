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
    public class EditTrainerCommand : AsyncCommandBase
    {
        private readonly NavigationService<TrainersListViewModel> navigationService;
        private readonly EmployeeStore _employeeStore;
        private EditTrainerViewModel _editTrainerViewModel;

        public EditTrainerCommand(NavigationService<TrainersListViewModel> navigationService, EditTrainerViewModel editTrainerViewModel, EmployeeStore employeeStore)
        {
            this.navigationService = navigationService;
            _employeeStore = employeeStore;
            _editTrainerViewModel = editTrainerViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            emp.Employee employee = new emp.Employee()
            {
                Id=_employeeStore.SelectedEmployee!.Id,
                FullName = _editTrainerViewModel.FullName,
                BirthDate = _editTrainerViewModel.BirthDate,
                GenderMale = _editTrainerViewModel.GenderMale,
                IsActive = true,
                IsTrainer = true,
                IsSecrtaria = false,
                ParcentValue = _editTrainerViewModel.ParcentValue,
                SalaryValue = _editTrainerViewModel.SalaryValue,
                Phone = _editTrainerViewModel.Phone,
                StartDate = _editTrainerViewModel.StartDate,
                Position = _editTrainerViewModel.Position,
            };
            await _employeeStore.DeleteConnectedSports(employee.Id);
            foreach (var SportListItem in _editTrainerViewModel.SportList)
            {
                if (SportListItem.IsSelected)
                    employee.Sports!.Add(SportListItem.sport);
            }
            await _employeeStore.Update(employee);
            navigationService.ReNavigate();
        }

    }

}
