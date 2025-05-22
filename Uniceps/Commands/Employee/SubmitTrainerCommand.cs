//using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using emp = Uniceps.Core.Models.Employee;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;

namespace Uniceps.Commands.Employee
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
            _addTrainerViewModel.PropertyChanged += _addTrainerViewModel_PropertyChanged;
        }

        private void _addTrainerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_addTrainerViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _addTrainerViewModel.CanSubmit && !string.IsNullOrEmpty(_addTrainerViewModel.FullName) && _addTrainerViewModel.Phone!.Trim().Length > 9 && base.CanExecute(null);

        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                Core.Models.Employee.Employee employee = new emp.Employee()
                {
                    FullName = _addTrainerViewModel.FullName,
                    Balance = _addTrainerViewModel.Balance,
                    BirthDate = _addTrainerViewModel.Year!.year,
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
                navigationService.ReNavigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
