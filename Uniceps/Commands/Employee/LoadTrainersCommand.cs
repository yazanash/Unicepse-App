using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.ViewModels;
using Uniceps.Stores;

namespace Uniceps.Commands.Employee
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
                _trainersListViewModel.ErrorMessage = "خطأ في تحميل العاملين يرجى اعادة تشغيل البرنامج";
            }
            finally
            {
                _trainersListViewModel.IsLoading = false;
            }
        }
    }
}
