using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;

namespace Uniceps.Commands.Employee
{
    public class DeleteEmployeeCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly INavigationService _navigationService;
        public DeleteEmployeeCommand(EmployeeStore employeeStore, INavigationService navigationService)
        {
            _employeeStore = employeeStore;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is int employeeId)
            {
                if (MessageBox.Show("سيتم حذف هذا المدرب , هل انت متاكد", "تنبيه", MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    try
                    {
                        await _employeeStore.Delete(employeeId);
                        _navigationService.Navigate<TrainersListViewModel>();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
            }
        }
    }
}
