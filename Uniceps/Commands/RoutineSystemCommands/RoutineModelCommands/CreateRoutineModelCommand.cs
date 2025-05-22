using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands
{
    public class CreateRoutineModelCommand : AsyncCommandBase
    {
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly CreateRoutineViewModel _createRoutineViewModel;
        private readonly NavigationService<RoutineListViewModel> _navigationService;
        public CreateRoutineModelCommand(RoutineTempDataStore routineTempDataStore, CreateRoutineViewModel createRoutineViewModel, NavigationService<RoutineListViewModel> navigationService)
        {
            _routineTempDataStore = routineTempDataStore;
            _createRoutineViewModel = createRoutineViewModel;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            RoutineModel routineModel = new()
            {
                Name = _createRoutineViewModel.Name!,
                Level = _createRoutineViewModel.Level!
            };
            await _routineTempDataStore.Add(routineModel);
            _navigationService.ReNavigate();
        }
    }
}
