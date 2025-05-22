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
        private readonly ExercisesListItemViewModel _exercisesViewModel;
        public CreateRoutineItemsModelCommand(DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore, ExercisesListItemViewModel exercisesViewModel)
        {
            _dayGroupDataStore = dayGroupDataStore;
            _routineItemDataStore = routineItemDataStore;
            _exercisesViewModel = exercisesViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            //int i = 0;
            //foreach (var item in _exercisesViewModel.ExercisesList.Where(x => x.IsChecked))
            //{
            RoutineItemModel routineItemModel = new()
            {
                Day = _dayGroupDataStore.SelectedDayGroup!,
                DayId = _dayGroupDataStore.SelectedDayGroup!.Id,
                Exercise = _exercisesViewModel.Exercises,
                ExerciseId = _exercisesViewModel.Exercises.Id,
                Order = _routineItemDataStore.RoutineItems.Count() + 1,
            };
            _exercisesViewModel.IsChecked = true;

            await _routineItemDataStore.Add(routineItemModel);
        }
    }
}
