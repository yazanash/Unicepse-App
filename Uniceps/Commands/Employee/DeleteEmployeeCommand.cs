using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;

namespace Uniceps.Commands.Employee
{
    public class DeleteEmployeeCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly NavigationService<TrainersListViewModel> _navigationService;
        public DeleteEmployeeCommand(EmployeeStore employeeStore, NavigationService<TrainersListViewModel> navigationService)
        {
            _employeeStore = employeeStore;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (MessageBox.Show("سيتم حذف هذا المدرب , هل انت متاكد", "تنبيه", MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning) == MessageBoxResult.Yes)
                try
                {
                    if (_employeeStore.SelectedEmployee!.IsTrainer)
                        await _employeeStore.DeleteConnectedSports(_employeeStore.SelectedEmployee!.Id);
                    await _employeeStore.Delete(_employeeStore.SelectedEmployee!.Id);
                    _navigationService.ReNavigate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }
    }
}
