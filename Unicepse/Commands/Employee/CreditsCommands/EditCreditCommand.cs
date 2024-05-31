using Unicepse.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels.Employee.CreditViewModels;
using Unicepse.navigation;

namespace Unicepse.Commands.Employee.CreditsCommands
{
    public class EditCreditCommand : AsyncCommandBase
    {
        private readonly NavigationService<CreditListViewModel> navigationService;
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditsDataStore;
        private EditCreditDetailsViewModel _creditDetailsViewModel;

        public EditCreditCommand(NavigationService<CreditListViewModel> navigationService, EmployeeStore employeeStore, CreditsDataStore creditsDataStore, EditCreditDetailsViewModel creditDetailsViewModel)
        {
            this.navigationService = navigationService;
            _employeeStore = employeeStore;
            _creditsDataStore = creditsDataStore;
            _creditDetailsViewModel = creditDetailsViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            Credit credit = new Credit()
            {
                Id = _creditsDataStore.SelectedCredit!.Id,
                CreditValue = _creditDetailsViewModel.CreditValue,
                Date = _creditDetailsViewModel.CreditDate,
                EmpPerson = _employeeStore.SelectedEmployee!,
                Description = _creditDetailsViewModel.Description,
            };

            await _creditsDataStore.Update(credit);
            navigationService.ReNavigate();
        }
    }
}
