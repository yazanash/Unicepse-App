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
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands
{
    public class CreateRoutineModelCommand : AsyncCommandBase
    {
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly CreateRoutineViewModel _createRoutineViewModel;
        public CreateRoutineModelCommand(RoutineTempDataStore routineTempDataStore, CreateRoutineViewModel createRoutineViewModel)
        {
            _routineTempDataStore = routineTempDataStore;
            _createRoutineViewModel = createRoutineViewModel;
            _createRoutineViewModel.PropertyChanged += _createRoutineViewModel_PropertyChanged;
        }

        private void _createRoutineViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_createRoutineViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }
        public override bool CanExecute(object? parameter)
        {

            return _createRoutineViewModel.CanSubmit && !string.IsNullOrEmpty(_createRoutineViewModel.Name) && base.CanExecute(null);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            RoutineModel routineModel = new()
            {
                Name = _createRoutineViewModel.Name!,
                Level = _createRoutineViewModel.Level!
            };
            await _routineTempDataStore.Add(routineModel);
            _createRoutineViewModel.OnRoutineCreated();
        }
    }
}
