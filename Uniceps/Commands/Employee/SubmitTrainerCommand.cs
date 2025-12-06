//using PlatinumGymPro.Models;
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
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.Views;
using Emp = Uniceps.Core.Models.Employee;

namespace Uniceps.Commands.Employee
{
    public class SubmitTrainerCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private AddTrainerViewModel _addTrainerViewModel;

        public SubmitTrainerCommand(AddTrainerViewModel addTrainerViewModel, EmployeeStore employeeStore)
        {
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
                Core.Models.Employee.Employee employee = new Emp.Employee()
                {
                    FullName = _addTrainerViewModel.FullName,
                    Balance = _addTrainerViewModel.Balance,
                    BirthDate = _addTrainerViewModel.Year?.year ?? DateTime.Now.Year,
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
                if (MessageBox.Show("تم اضافة المدرب بنجاح ... هل تريد اضافة مدرب اخر؟", "تم بنجاح", MessageBoxButton.YesNo, MessageBoxImage.Information)
                   == MessageBoxResult.Yes)
                {
                    _addTrainerViewModel.ClearForm();
                }
                else
                _addTrainerViewModel.OnTrainerCreated();
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
