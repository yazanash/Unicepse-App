using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Expenses;
using Uniceps.Stores;
using Uniceps.ViewModels.Expenses;
using Uniceps.ViewModels.OtherRevenueViewModels;

namespace Uniceps.Commands.RevenueCommands
{
    public class SubmitRevenueCommand : AsyncCommandBase
    {
        private readonly OtherRevenuesDataStore _otherRevenuesDataStore;
        private AddRevenueViewModel _addRevenueViewModel;

        public SubmitRevenueCommand(OtherRevenuesDataStore otherRevenuesDataStore, AddRevenueViewModel addRevenueViewModel)
        {
            _otherRevenuesDataStore = otherRevenuesDataStore;
            _addRevenueViewModel = addRevenueViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            OtherRevenue revenue = new OtherRevenue()
            {
                Service = _addRevenueViewModel.Service ?? "",
                Description = _addRevenueViewModel.Description ?? "",
                Date = _addRevenueViewModel.Date,
                Amount = _addRevenueViewModel.Amount,
                CustomerName = _addRevenueViewModel.CustomerName ?? "",
            };
            if(_addRevenueViewModel.IsEditMode&& _addRevenueViewModel.OtherRevenue != null)
            {
                revenue.Id = _addRevenueViewModel.OtherRevenue.Id;
                await _otherRevenuesDataStore.Update(revenue);
            }
            else
                await _otherRevenuesDataStore.Add(revenue);
            _addRevenueViewModel.OnRevenueCreated();
        }
    }
}
