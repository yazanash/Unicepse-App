using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands
{
    public class UpdateRoutineModelCommand : AsyncCommandBase
    {
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly RoutineDetailsViewModel _routineDetailsViewModel;
        public UpdateRoutineModelCommand(RoutineTempDataStore routineTempDataStore, RoutineDetailsViewModel routineDetailsViewModel)
        {
            _routineTempDataStore = routineTempDataStore;
            _routineDetailsViewModel = routineDetailsViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_routineTempDataStore.SelectedRoutine != null)
            {
                RoutineModel routineModel = new()
                {
                    Id = _routineTempDataStore.SelectedRoutine.Id,
                    Name = _routineDetailsViewModel.Name!,
                    Level = _routineDetailsViewModel.Level!
                };
                await _routineTempDataStore.Update(routineModel);
                _routineDetailsViewModel.IsEditable = false;
                
            }
           
        }
    }
}
