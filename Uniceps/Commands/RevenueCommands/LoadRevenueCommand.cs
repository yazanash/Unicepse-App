using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Stores;
using Uniceps.ViewModels.Expenses;
using Uniceps.ViewModels.OtherRevenueViewModels;

namespace Uniceps.Commands.RevenueCommands
{
    public class LoadRevenueCommand : AsyncCommandBase
    {
        private readonly OtherRevenuesDataStore _otherRevenuesDataStore;
        private readonly RevenueListViewModel _revenueListViewModel;

        public LoadRevenueCommand(OtherRevenuesDataStore otherRevenuesDataStore, RevenueListViewModel revenueListViewModel)
        {
            _otherRevenuesDataStore = otherRevenuesDataStore;
            _revenueListViewModel = revenueListViewModel;
        }
        public async override Task ExecuteAsync(object? parameter)
        {
            _revenueListViewModel.ErrorMessage = null;
            _revenueListViewModel.IsLoading = true;

            try
            {
                await _otherRevenuesDataStore.GetAll();
            }
            catch (Exception)
            {
                _revenueListViewModel.ErrorMessage = "خطأ في تحميل الايرادات يرجى اعادة تشغيل البرنامج";
            }
            finally
            {
                _revenueListViewModel.IsLoading = false;
            }
        }
    }
}
