using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands
{
    public class LoadRoutineModelsCommand : AsyncCommandBase
    {
        private readonly RoutineTempDataStore _routineTempDataStore;

        public LoadRoutineModelsCommand(RoutineTempDataStore routineTempDataStore)
        {
            _routineTempDataStore = routineTempDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _routineTempDataStore.GetAll();
        }
    }
}
