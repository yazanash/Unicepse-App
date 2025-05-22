using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.Core.Models.Employee;

namespace Uniceps.Commands.Employee.CreditsCommands
{
    public class SubmitCreditCommand : AsyncCommandBase
    {
        private readonly NavigationService<CreditListViewModel> navigationService;
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditsDataStore;
        private CreditDetailsViewModel _creditDetailsViewModel;

        public SubmitCreditCommand(NavigationService<CreditListViewModel> navigationService, EmployeeStore employeeStore, CreditsDataStore creditsDataStore, CreditDetailsViewModel creditDetailsViewModel)
        {
            this.navigationService = navigationService;
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
            Credit credit = new Credit()
            {
                CreditValue = _creditDetailsViewModel.CreditValue,
                Date = _creditDetailsViewModel.CreditDate,
                Description = _creditDetailsViewModel.Description,
            };
            credit.EmpPerson = new Core.Models.Employee.Employee() { Id = _employeeStore.SelectedEmployee!.Id };

            await _creditsDataStore.Add(credit);
            navigationService.ReNavigate();
        }

    }
}
