using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.Commands.RoutineSystemCommands.DayGroupCommands
{
    public class LoadDayGroupsCommand : AsyncCommandBase
    {
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly RoutineTempDataStore _routineTempDataStore;

        public LoadDayGroupsCommand(DayGroupDataStore dayGroupDataStore, RoutineTempDataStore routineTempDataStore)
        {
            _dayGroupDataStore = dayGroupDataStore;
            _routineTempDataStore = routineTempDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_routineTempDataStore.SelectedRoutine != null)
                await _dayGroupDataStore.GetAllById(_routineTempDataStore.SelectedRoutine.Id);
        }
    }
}
