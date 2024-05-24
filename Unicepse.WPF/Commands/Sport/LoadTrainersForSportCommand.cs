using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels;
using Unicepse.WPF.ViewModels.SportsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.WPF.Commands.Sport
{
    public class LoadTrainersForSportCommand : AsyncCommandBase
    {
        private readonly EmployeeStore _trainerStore;
        private readonly ListingViewModelBase _addSportViewModel;
        public LoadTrainersForSportCommand(EmployeeStore trainerStore, ListingViewModelBase addSportViewModel)
        {
            _trainerStore = trainerStore;
            _addSportViewModel = addSportViewModel;
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            _addSportViewModel.ErrorMessage = null;
            _addSportViewModel.IsLoading = true;

            try
            {
                await _trainerStore.GetAll();
            }
            catch (Exception)
            {
                _addSportViewModel.ErrorMessage = "Failed to load trainers . Please restart the application.";
            }
            finally
            {
                _addSportViewModel.IsLoading = false;
            }
        }
    }
}
