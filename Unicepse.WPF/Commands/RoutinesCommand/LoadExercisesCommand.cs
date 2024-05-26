using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.WPF.Commands.RoutinesCommand
{
    public class LoadExercisesCommand : AsyncCommandBase
    {
        private readonly RoutineDataStore _routineDataStore;
        private readonly ListingViewModelBase _routineListing;

        public LoadExercisesCommand(RoutineDataStore routineDataStore, ListingViewModelBase routineListing)
        {
            _routineDataStore = routineDataStore;
            _routineListing = routineListing;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _routineListing.ErrorMessage = null;
            _routineListing.IsLoading = true;

            try
            {

                await _routineDataStore.GetAllExercises();
            }
            catch (Exception)
            {
                _routineListing.ErrorMessage = "Failed to load Players. Please restart the application.";
            }
            finally
            {
                _routineListing.IsLoading = false;
            }
        }
    }
}
