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
    public class SubmitRoutineCommand : AsyncCommandBase
    {
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationService<RoutinePlayerViewModels> _navigationService;
        private readonly SelectRoutineDaysMuscleGroupViewModel selectRoutineDaysMuscleGroupViewModel;
        public SubmitRoutineCommand(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationService<RoutinePlayerViewModels> navigationService, SelectRoutineDaysMuscleGroupViewModel selectRoutineDaysMuscleGroupViewModel)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _navigationService = navigationService;
            this.selectRoutineDaysMuscleGroupViewModel = selectRoutineDaysMuscleGroupViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            PlayerRoutine playerRoutine = new()
            {
                RoutineNo = selectRoutineDaysMuscleGroupViewModel.Number,
                RoutineData =selectRoutineDaysMuscleGroupViewModel.Date,
                Player=_playersDataStore.SelectedPlayer!.Player,
                
            };
            
            playerRoutine.RoutineSchedule.AddRange(_routineDataStore.RoutineItems);
           foreach (var item in selectRoutineDaysMuscleGroupViewModel.DayGroupList)
            {
                playerRoutine.DaysGroupMap!.Add(item.SelectedDay, item.Groups);
            }
           
            await _routineDataStore.Add(playerRoutine);
            MessageBox.Show("Player Routine Added Successfully");
            _navigationService.ReNavigate();
        }
    }
}
