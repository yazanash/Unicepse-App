using Unicepse.Core.Models.Employee;
using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.Employee.CreditViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.WPF.navigation;

namespace Unicepse.WPF.Commands.Employee.CreditsCommands
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
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            Credit credit= new Credit()
            {
                CreditValue = _creditDetailsViewModel.CreditValue,
                 Date = _creditDetailsViewModel.CreditDate,
                Description = _creditDetailsViewModel.Description,
            };
            credit.EmpPerson = new Unicepse.Core.Models.Employee.Employee() { Id = _employeeStore.SelectedEmployee!.Id };
            
            await _creditsDataStore.Add(credit);
            navigationService.ReNavigate();
        }

    }
}
