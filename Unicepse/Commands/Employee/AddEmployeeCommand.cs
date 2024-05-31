using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using emp = Unicepse.Core.Models.Employee;
using Unicepse.ViewModels.Employee.TrainersViewModels;
using Unicepse.Stores;
using Unicepse.navigation;

namespace Unicepse.Commands.Employee
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
            _addEmployeeViewModel.PropertyChanged += _addEmployeeViewModel_PropertyChanged;
        }

        private void _addEmployeeViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_addEmployeeViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _addEmployeeViewModel.CanSubmit && !string.IsNullOrEmpty(_addEmployeeViewModel.FullName) && _addEmployeeViewModel.Phone!.Trim().Length > 9 && base.CanExecute(null);

        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                emp.Employee employee = new emp.Employee()
                {
                    FullName = _addEmployeeViewModel.FullName,
                    Balance = _addEmployeeViewModel.Balance,
                    BirthDate = _addEmployeeViewModel.Year!.year,
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
