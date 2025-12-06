using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands
{
    public class LoadRoutineItemsModelCommand : AsyncCommandBase
    {
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly RoutineItemDataStore _routineItemDataStore;

        public LoadRoutineItemsModelCommand(DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore)
        {
            _dayGroupDataStore = dayGroupDataStore;
            _routineItemDataStore = routineItemDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_dayGroupDataStore.SelectedDayGroup != null)
                await _routineItemDataStore.GetAllById(_dayGroupDataStore.SelectedDayGroup!.Id);
        }
    }
}
