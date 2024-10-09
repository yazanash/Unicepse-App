using Newtonsoft.Json.Linq;
using Unicepse.Core.Models.TrainingProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels.RoutineViewModels;
using Unicepse.navigation;

namespace Unicepse.Commands.RoutinesCommand
{
    public class UpdateRoutineCommand : AsyncCommandBase
    {
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationService<RoutinePlayerViewModels> _navigationService;
        private readonly EditSelectRoutineDaysMuscleGroupViewModel _editSelectRoutineDaysMuscleGroupViewModel;
        public UpdateRoutineCommand(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationService<RoutinePlayerViewModels> navigationService, EditSelectRoutineDaysMuscleGroupViewModel editSelectRoutineDaysMuscleGroupViewModel)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _navigationService = navigationService;
            _editSelectRoutineDaysMuscleGroupViewModel = editSelectRoutineDaysMuscleGroupViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            PlayerRoutine playerRoutine = _routineDataStore.SelectedRoutine!;
            //await _routineDataStore.DeleteRoutineItems(playerRoutine.Id);
            playerRoutine.RoutineSchedule.Clear();
            playerRoutine.RoutineSchedule.AddRange(_routineDataStore.RoutineItems);
            playerRoutine.DaysGroupMap!.Clear();

            foreach (var item in _editSelectRoutineDaysMuscleGroupViewModel.DayGroupList)
            {
                playerRoutine.DaysGroupMap!.Add(item.SelectedDay, item.Groups);
            }

            playerRoutine.RoutineData = _editSelectRoutineDaysMuscleGroupViewModel.Date;
            playerRoutine.RoutineNo = _editSelectRoutineDaysMuscleGroupViewModel.Number;
            playerRoutine.IsTemplate = _editSelectRoutineDaysMuscleGroupViewModel.IsTemplate;
            await _routineDataStore.Update(playerRoutine);

            _navigationService.ReNavigate();
        }
    }

}
