using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emp = Uniceps.Core.Models.Employee;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using System.Windows;

namespace Uniceps.Commands.Employee
{
    public class EditEmployeeCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private EditEmployeeViewModel _editEmployeeViewModel;

        public EditEmployeeCommand( EditEmployeeViewModel editEmployeeViewModel, EmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
            _editEmployeeViewModel = editEmployeeViewModel;
            _editEmployeeViewModel.PropertyChanged += _editEmployeeViewModel_PropertyChanged;
        }

        private void _editEmployeeViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_editEmployeeViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _editEmployeeViewModel.CanSubmit && !string.IsNullOrEmpty(_editEmployeeViewModel.FullName) && _editEmployeeViewModel.Phone!.Trim().Length > 9 && base.CanExecute(null);

        }
        public override async Task ExecuteAsync(object? parameter)
        {
            Core.Models.Employee.Employee employee = new Emp.Employee()
            {
                Id = _employeeStore.SelectedEmployee!.Id,
                FullName = _editEmployeeViewModel.FullName,
                BirthDate = _editEmployeeViewModel.Year?.year ?? DateTime.Now.Year,
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
            MessageBox.Show("تم التعديل بنجاح");
            _editEmployeeViewModel.OnEmployeeUpdated();
        }

    }
}
