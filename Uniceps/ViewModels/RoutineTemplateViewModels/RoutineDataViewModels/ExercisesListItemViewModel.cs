using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.utlis.common;
using Uniceps.Stores;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels
{
    public class ExercisesListItemViewModel : ViewModelBase
    {
        public Exercises Exercises;
        public string? ExerciseImage => Exercises.ImagePath;
        public string? ExerciseName => Exercises.Name;
        public string? Group { get; set; }
        public int Id => Exercises.Id;
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; OnPropertyChanged(nameof(IsChecked)); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        public ICommand? AddItemCommand { get; }
        private readonly RoutineItemDataStore _routineItemDataStore;
        private readonly DayGroupDataStore _dayGroupDataStore;
        public ExercisesListItemViewModel(Exercises exercises, RoutineItemDataStore routineItemDataStore, DayGroupDataStore dayGroupDataStore)
        {
            Exercises = exercises;
            switch (Exercises.MuscleGroupId)
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
            _routineItemDataStore = routineItemDataStore;
            _dayGroupDataStore = dayGroupDataStore;
           
            
        }
    }
}
