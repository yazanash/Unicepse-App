using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Core.Models.Employee;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.CreditViewModels;

namespace Uniceps.Commands.Employee.CreditsCommands
{
    public class EditCreditCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditsDataStore;
        private EditCreditDetailsViewModel _creditDetailsViewModel;

        public EditCreditCommand( EmployeeStore employeeStore, CreditsDataStore creditsDataStore, EditCreditDetailsViewModel creditDetailsViewModel)
        {
            _employeeStore = employeeStore;
            _creditsDataStore = creditsDataStore;
            _creditDetailsViewModel = creditDetailsViewModel;
            _creditDetailsViewModel.PropertyChanged += _creditDetailsViewModel_PropertyChanged;
        }

        private void _creditDetailsViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_creditDetailsViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }
        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _creditDetailsViewModel.CreditValue > 0;
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                Credit credit = new Credit()
                {
                    Id = _creditsDataStore.SelectedCredit!.Id,
                    CreditValue = _creditDetailsViewModel.CreditValue,
                    Date = _creditDetailsViewModel.CreditDate,
                    Description = _creditDetailsViewModel.Description,
                    EmpPersonSyncId = _employeeStore.SelectedEmployee!.SyncId,
                    EmpPersonId = _employeeStore.SelectedEmployee!.Id,
                };

                await _creditsDataStore.Update(credit);
                _creditDetailsViewModel.OnCreditUpdated();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }
    }
}
