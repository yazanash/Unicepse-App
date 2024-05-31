using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Commands.RoutinesCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Stores;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class ExercisesListItemViewModel : ViewModelBase
    {
        Exercises Exercises;
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        public string ExerciseImage => "pack://application:,,,/Resources/Assets/Exercises/" + Exercises.GroupId + "/" + Exercises.ImageId + ".png";
        public string? ExerciseName => Exercises.Name;
        public int Id => Exercises.Id;
        public ExercisesListItemViewModel(Exercises exercises, RoutineDataStore routineDataStore, PlayersDataStore playersDataStore)
        {
            Exercises = exercises;
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            AddToRoutineCommand = new AddExercisesToRoutineItemsCommand(_playersDataStore, _routineDataStore, Exercises, this);

        }
        public ICommand? AddToRoutineCommand { get; }
    }
}
