using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.Commands.RoutineSystemCommands.DayGroupCommands
{
    public class CreateDayGroupCommand : AsyncCommandBase
    {
        private readonly DayGroupDataStore _dataStore;
        private readonly RoutineTempDataStore _routineTempDataStore;

        public CreateDayGroupCommand(DayGroupDataStore dataStore, RoutineTempDataStore routineTempDataStore)
        {
            _dataStore = dataStore;
            _routineTempDataStore = routineTempDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            DayGroup dayGroup = new DayGroup();
            dayGroup.Name = "Untitled";
            dayGroup.Routine = _routineTempDataStore.SelectedRoutine!;
            dayGroup.RoutineId = _routineTempDataStore.SelectedRoutine!.Id;
            await _dataStore.Add(dayGroup);

        }
    }
}
