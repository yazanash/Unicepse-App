using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.Commands.RoutineSystemCommands.SetModelsCommands
{
    public class AddSetModelCommand : AsyncCommandBase
    {
        private readonly SetsModelDataStore _setsModelDataStore;
        private readonly RoutineItemDataStore _routineItemDataStore;

        public AddSetModelCommand(SetsModelDataStore setsModelDataStore, RoutineItemDataStore routineItemDataStore)
        {
            _setsModelDataStore = setsModelDataStore;
            _routineItemDataStore = routineItemDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            int reps = _setsModelDataStore.SetModels.Count()>0?_setsModelDataStore.SetModels.LastOrDefault()!.Repetition:0;
            SetModel setModel = new SetModel()
            {
                Repetition = reps,
                RoundIndex = _setsModelDataStore.SetModels.Count(),
                RoutineItem = _routineItemDataStore.SelectedRoutineItem!,
                RoutineItemId = _routineItemDataStore.SelectedRoutineItem!.Id,
            };
            await _setsModelDataStore.Add(setModel);
        }
    }
}
