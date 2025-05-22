using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores.RoutineStores;
using Uniceps.Core.Models.RoutineModels;

namespace Uniceps.Commands.RoutineSystemCommands.SetModelsCommands
{
    class LoadSetModelsCommand : AsyncCommandBase
    {
        private readonly SetsModelDataStore _setsModelDataStore;
        private readonly RoutineItemDataStore _routineItemDataStore;

        public LoadSetModelsCommand(SetsModelDataStore setsModelDataStore, RoutineItemDataStore routineItemDataStore)
        {
            _setsModelDataStore = setsModelDataStore;
            _routineItemDataStore = routineItemDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_routineItemDataStore.SelectedRoutineItem != null)
                await _setsModelDataStore.GetAllById(_routineItemDataStore.SelectedRoutineItem!.Id);
        }
    }
}
