using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Emp = Uniceps.Core.Models.Employee;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;

namespace Uniceps.Commands.Employee
{
    public class EditTrainerCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private EditTrainerViewModel _editTrainerViewModel;

        public EditTrainerCommand( EditTrainerViewModel editTrainerViewModel, EmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
            _editTrainerViewModel = editTrainerViewModel;
            _editTrainerViewModel.PropertyChanged += _editTrainerViewModel_PropertyChanged;
        }

        private void _editTrainerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_editTrainerViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _editTrainerViewModel.CanSubmit && !string.IsNullOrEmpty(_editTrainerViewModel.FullName) && _editTrainerViewModel.Phone!.Trim().Length > 9 && base.CanExecute(null);

        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                Core.Models.Employee.Employee employee = new Emp.Employee()
                {
                    Id = _employeeStore.SelectedEmployee!.Id,
                    FullName = _editTrainerViewModel.FullName,
                    BirthDate = _editTrainerViewModel.Year?.year ?? DateTime.Now.Year,
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
                MessageBox.Show("تم التعديل بنجاح");
                _editTrainerViewModel.OnTrainerUpdated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

}
