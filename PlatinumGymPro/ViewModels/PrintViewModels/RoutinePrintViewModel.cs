using PlatinumGymPro.Enums;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.RoutineViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.PrintViewModels
{
    public class RoutinePrintViewModel :ViewModelBase
    {
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly SelectRoutineDaysMuscleGroupViewModel _selectRoutineDaysMuscleGroupViewModel;
        public RoutinePrintViewModel(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, SelectRoutineDaysMuscleGroupViewModel selectRoutineDaysMuscleGroupViewModel)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _selectRoutineDaysMuscleGroupViewModel = selectRoutineDaysMuscleGroupViewModel;
            _routineExercisesItemsViewModels = new ObservableCollection<RoutineExercisesItemsViewModel>();
            foreach(var routine in _routineDataStore.RoutineItems)
            {
                RoutineExercisesItemsViewModel routineExercisesItemsViewModel = new(routine);
                _routineExercisesItemsViewModels.Add(routineExercisesItemsViewModel);
            }
            
        }

        private readonly ObservableCollection<RoutineExercisesItemsViewModel> _routineExercisesItemsViewModels;
        public IEnumerable<RoutineExercisesItemsViewModel> LegsExercisesList
            => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Legs);
        public IEnumerable<RoutineExercisesItemsViewModel> ChestExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Chest);
        public IEnumerable<RoutineExercisesItemsViewModel> BackExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Back);
        public IEnumerable<RoutineExercisesItemsViewModel> ShouldersExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Shoulders);
        public IEnumerable<RoutineExercisesItemsViewModel> BicepsExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Biceps);
        public IEnumerable<RoutineExercisesItemsViewModel> TricepsExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Triceps);
        public IEnumerable<RoutineExercisesItemsViewModel> CalvesExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Calves);
        public IEnumerable<RoutineExercisesItemsViewModel> AbsExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Abs);
        public DateTime Date=>_selectRoutineDaysMuscleGroupViewModel.Date;
        public int Id => _selectRoutineDaysMuscleGroupViewModel.Number;
        public string? FullName => _playersDataStore.SelectedPlayer!.FullName;

        public string day1 => _selectRoutineDaysMuscleGroupViewModel.DayGroupList.SingleOrDefault(x=>x.SelectedDay==1)!.GetGroups();
        public string day2 => _selectRoutineDaysMuscleGroupViewModel.DayGroupList.SingleOrDefault(x => x.SelectedDay == 2)!.GetGroups();
        public string day3 => _selectRoutineDaysMuscleGroupViewModel.DayGroupList.SingleOrDefault(x => x.SelectedDay == 3)!.GetGroups();
        public string day4 => _selectRoutineDaysMuscleGroupViewModel.DayGroupList.SingleOrDefault(x => x.SelectedDay == 4)!.GetGroups();
        public string day5 => _selectRoutineDaysMuscleGroupViewModel.DayGroupList.SingleOrDefault(x => x.SelectedDay == 5)!.GetGroups();
        public string day6 => _selectRoutineDaysMuscleGroupViewModel.DayGroupList.SingleOrDefault(x => x.SelectedDay == 6)!.GetGroups();
    }
}
