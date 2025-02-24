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
        public Exercises Exercises;
        private readonly RoutineDataStore? _routineDataStore;
        private readonly PlayersDataStore? _playersDataStore;
        public string ExerciseImage => "pack://application:,,,/Resources/Assets/Exercises/" + Exercises.GroupId + "/" + Exercises.ImageId + ".jpg";
        public string? ExerciseName => Exercises.Name;
        public string? Group { get; set; }
        public int Id => Exercises.Id;
        public ExercisesListItemViewModel(Exercises exercises, RoutineDataStore routineDataStore, PlayersDataStore playersDataStore)
        {
            Exercises = exercises;
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            AddToRoutineCommand = new AddExercisesToRoutineItemsCommand(_playersDataStore, _routineDataStore, Exercises, this);
            
        }
        public ExercisesListItemViewModel(Exercises exercises)
        {
            Exercises = exercises;
            switch (Exercises.GroupId)
            {
                case (int)EMuscleGroup.Abs:
                    Group = "معدة";
                    break;
                case (int)EMuscleGroup.Back:
                    Group = "ظهر";
                    break;
                case (int)EMuscleGroup.Triceps:
                    Group = "ترايسبس";
                    break;
                case (int)EMuscleGroup.Biceps:
                    Group = "بايسيبس";
                    break;
                case (int)EMuscleGroup.Shoulders:
                    Group = "الأكتاف";
                    break;
                case (int)EMuscleGroup.Legs:
                    Group = "الارجل";
                    break;
                case (int)EMuscleGroup.Calves:
                    Group = "بطات الارجل";
                    break;
            }
        }
        public ICommand? AddToRoutineCommand { get; }
    }
}
