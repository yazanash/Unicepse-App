using Newtonsoft.Json.Linq;
using PlatinumGym.Core.Models.TrainingProgram;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.RoutineViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.RoutinesCommand
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
            _editSelectRoutineDaysMuscleGroupViewModel= editSelectRoutineDaysMuscleGroupViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            PlayerRoutine playerRoutine = _routineDataStore.SelectedRoutine!;
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

            MessageBox.Show("Player Routine Updated Successfully");
            _navigationService.ReNavigate();
        }
    }

}
