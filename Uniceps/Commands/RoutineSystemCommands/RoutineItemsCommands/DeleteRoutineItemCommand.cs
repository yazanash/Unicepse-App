using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands
{
    public class DeleteRoutineItemCommand : AsyncCommandBase
    {
        private readonly RoutineItemDataStore _routineItemDataStore;

        public DeleteRoutineItemCommand(RoutineItemDataStore routineItemDataStore)
        {
            _routineItemDataStore = routineItemDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _routineItemDataStore.Delete(_routineItemDataStore.SelectedRoutineItem!.Id);
        }
    }
}
