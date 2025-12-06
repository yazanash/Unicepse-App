using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Core.Exceptions;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.Views;
using Emp = Uniceps.Core.Models.Employee;

namespace Uniceps.Commands.Employee
{
    public class AddEmployeeCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private AddEmployeeViewModel _addEmployeeViewModel;

        public AddEmployeeCommand( AddEmployeeViewModel addEmployeeViewModel, EmployeeStore employeeStore)
        {
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
                Core.Models.Employee.Employee employee = new Emp.Employee()
                {
                    FullName = _addEmployeeViewModel.FullName,
                    Balance = _addEmployeeViewModel.Balance,
                    BirthDate = _addEmployeeViewModel.Year?.year ?? DateTime.Now.Year,
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
               if( MessageBox.Show("تم اضافة الموظف بنجاح ... هل تريد اضافة موظف اخر؟","تم بنجاح",MessageBoxButton.YesNo,MessageBoxImage.Information)
                    == MessageBoxResult.Yes)
                {
                    _addEmployeeViewModel.ClearForm();
                }else
                    _addEmployeeViewModel.OnEmployeeCreated();
            }
            catch (FreeLimitException)
            {
                PremiumViewDialog premiumViewDialog = new PremiumViewDialog();
                premiumViewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
