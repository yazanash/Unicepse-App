using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.WPF.Commands.Employee
{
    public class LoadTrainersCommand : AsyncCommandBase
    {

        private readonly EmployeeStore _employeeStore;
        private readonly ListingViewModelBase _trainersListViewModel;

        public LoadTrainersCommand(EmployeeStore employeeStore, ListingViewModelBase trainersListViewModel)
        {
            _employeeStore = employeeStore;
            _trainersListViewModel = trainersListViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _trainersListViewModel.ErrorMessage = null;
            _trainersListViewModel.IsLoading = true;

            try
            {
                await _employeeStore.GetAll();
            }
            catch (Exception)
            {
                _trainersListViewModel.ErrorMessage = "Failed to load Employees . Please restart the application.";
            }
            finally
            {
                _trainersListViewModel.IsLoading = false;
            }
        }
    }
}
