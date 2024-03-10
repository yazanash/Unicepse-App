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
        public SubmitRoutineCommand(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationService<RoutinePlayerViewModels> navigationService)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            PlayerRoutine playerRoutine = new()
            {
                RoutineNo = 1,
                RoutineData = DateTime.Now,
                Player=_playersDataStore.SelectedPlayer!.Player,
            };
            playerRoutine.RoutineSchedule.AddRange(_routineDataStore.RoutineItems);
           
            await _routineDataStore.Add(playerRoutine);
            MessageBox.Show("Player Routine Added Successfully");
            _navigationService.ReNavigate();
        }
    }
}
