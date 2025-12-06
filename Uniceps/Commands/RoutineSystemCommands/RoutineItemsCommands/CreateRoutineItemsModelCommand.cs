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
        public CreateRoutineItemsModelCommand(DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore, ExercisesListViewModel exercisesViewModel)
        {
            _dayGroupDataStore = dayGroupDataStore;
            _routineItemDataStore = routineItemDataStore;
            _exercisesViewModel = exercisesViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            int i = 1;
            foreach (var item in _exercisesViewModel._exercisesListItemViewModel.Where(x=>x.IsSelected).OrderBy(x=>x.SelectedAt).Select(x=>x.Exercises))
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
            _exercisesViewModel.ClearSelection();
            _exercisesViewModel.OnRoutineItemsCreated();
            //_exercisesViewModel.ExercisesBufferListItemViewModel.Clear();
        }
    }
}
