using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels;

namespace Unicepse.Commands.RoutinesCommand
{
    public class LoadAllRoutinesCommand : AsyncCommandBase
    {
        private readonly RoutineDataStore _routineDataStore;
        private readonly ListingViewModelBase _routineListing;
        private readonly PlayersDataStore _playerDataStore;

        public LoadAllRoutinesCommand(RoutineDataStore routineDataStore, ListingViewModelBase routineListing, PlayersDataStore playerDataStore)
        {
            _routineDataStore = routineDataStore;
            _routineListing = routineListing;
            _playerDataStore = playerDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _routineListing.ErrorMessage = null;
            _routineListing.IsLoading = true;

            //try
            //{

            await _routineDataStore.GetAll(_playerDataStore.SelectedPlayer!.Player);
            //}
            //catch (Exception)
            //{
            //    _routineListing.ErrorMessage = "Failed to load Players. Please restart the application.";
            //}
            //finally
            //{
            //    _routineListing.IsLoading = false;
            //}
        }
    }
}
