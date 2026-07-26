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
    public class SubmitCreditCommand : AsyncCommandBase
    {
        private readonly CreditsDataStore _creditsDataStore;
        private CreateCreditViewModelWindow _creditDetailsViewModel;

        public SubmitCreditCommand( CreditsDataStore creditsDataStore, CreateCreditViewModelWindow creditDetailsViewModel)
        {
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
                    CreditValue = _creditDetailsViewModel.CreditValue,
                    Date = _creditDetailsViewModel.CreditDate,
                    Description = _creditDetailsViewModel.Description,
                    EmpPersonSyncId = _creditDetailsViewModel.SelectedEmployee!.SyncId,
                    EmpPersonId = _creditDetailsViewModel.SelectedEmployee!.Id,
                };
                if (_creditDetailsViewModel.IsEditMode&& _creditDetailsViewModel.Credit!=null)
                {
                    credit.Id = _creditDetailsViewModel.Credit.Id;
                    credit.EmpPersonId = _creditDetailsViewModel.Credit.EmpPersonId;
                    credit.EmpPersonSyncId = _creditDetailsViewModel.Credit.EmpPersonSyncId;
                    await _creditsDataStore.Update(credit);
                }
                else
                    await _creditsDataStore.Add(credit);
                _creditDetailsViewModel.OnCreditCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }

    }
}
