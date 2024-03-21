using PlatinumGym.Core.Models.Employee;
//using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.TrainersCommands
{
    public class SubmitTrainerCommand : AsyncCommandBase
    {
        private readonly NavigationService<TrainersListViewModel> navigationService;
        private readonly EmployeeStore _employeeStore;
        private AddTrainerViewModel _addTrainerViewModel;

        public SubmitTrainerCommand(NavigationService<TrainersListViewModel> navigationService, AddTrainerViewModel addTrainerViewModel, EmployeeStore employeeStore)
        {
            this.navigationService = navigationService;
            _employeeStore = employeeStore;
            _addTrainerViewModel = addTrainerViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            Employee employee = new Employee()
            {
                FullName = _addTrainerViewModel.FullName,
                Balance = _addTrainerViewModel.Balance,
                BirthDate = _addTrainerViewModel.BirthDate,
                GenderMale = _addTrainerViewModel.GenderMale,
                IsActive = true,
                IsTrainer = true,
                IsSecrtaria = false,
                ParcentValue = _addTrainerViewModel.ParcentValue,
                SalaryValue = _addTrainerViewModel.SalaryValue,
                Phone = _addTrainerViewModel.Phone,
                StartDate = _addTrainerViewModel.StartDate,
                Position = _addTrainerViewModel.Position,
            };
            foreach (var SportListItem in _addTrainerViewModel.SportList)
            {
                if (SportListItem.IsSelected)
                    employee.Sports!.Add(SportListItem.sport);
            }
            await _employeeStore.Add(employee);
            navigationService.Navigate();
        }

    }
}
