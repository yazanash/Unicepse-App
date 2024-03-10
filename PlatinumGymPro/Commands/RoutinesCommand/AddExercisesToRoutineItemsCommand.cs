using PlatinumGym.Core.Models.TrainingProgram;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.RoutinesCommand
{
    public class AddExercisesToRoutineItemsCommand : CommandBase
    {
        private readonly PlayersDataStore _playerStore;
        private readonly RoutineDataStore _routineDataStore;
        Exercises _exercises;
        public AddExercisesToRoutineItemsCommand(PlayersDataStore playerStore, RoutineDataStore routineDataStore , Exercises exercises, ViewModels.RoutineViewModels.ExercisesListItemViewModel exercisesListItemViewModel)
        {
            _playerStore = playerStore;
            _routineDataStore = routineDataStore;
           _exercises = exercises;
        }

        public override void Execute(object? parameter)
        {
            RoutineItems routineItems = new RoutineItems()
            {
                Exercises = _exercises,
                ItemOrder = _routineDataStore.RoutineItems.Where(x=>x.Exercises!.GroupId == _routineDataStore.SelectedMuscle!.Id).Count() + 1

        };
            _routineDataStore.AddRoutineItem(routineItems);
        }

    }
}
