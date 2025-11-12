using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands
{
    public class CreateRoutineItemsModelCommand : AsyncCommandBase
    {
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly RoutineItemDataStore _routineItemDataStore;
        private readonly ExercisesListViewModel _exercisesViewModel;
        private readonly NavigationService<RoutineDetailsViewModel> _navigationService;
        public CreateRoutineItemsModelCommand(DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore, ExercisesListViewModel exercisesViewModel, NavigationService<RoutineDetailsViewModel> navigationService)
        {
            _dayGroupDataStore = dayGroupDataStore;
            _routineItemDataStore = routineItemDataStore;
            _exercisesViewModel = exercisesViewModel;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            int i = 0;
            foreach (var item in _exercisesViewModel.ExercisesBufferListItemViewModel)
            {
                RoutineItemModel routineItemModel = new()
                {
                    Day = _dayGroupDataStore.SelectedDayGroup!,
                    DayId = _dayGroupDataStore.SelectedDayGroup!.Id,
                    Exercise = item,
                    ExerciseId = item.Id,
                    Order = i++,
                };
                await _routineItemDataStore.Add(routineItemModel);

            }
            _exercisesViewModel.ExercisesBufferListItemViewModel.Clear();
            _navigationService.Navigate();
        }
    }
}
