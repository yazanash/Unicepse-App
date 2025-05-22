using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;
using Uniceps.Commands.RoutineSystemCommands.ExerciseCommands;
using Uniceps.navigation;
using Uniceps.ViewModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.navigation.Stores;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class ExercisesViewModel : ListingViewModelBase
    {
        private readonly ExercisesDataStore _exercisesDataStore;
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly RoutineItemDataStore _routineItemDataStore;
        private readonly NavigationService<RoutineDetailsViewModel> _navigationService;
        private readonly NavigationStore _navigationStore;
        public RoutineItemsBufferListViewModel RoutineItemListViewModel { get; set; }
        private readonly ObservableCollection<ExercisesListItemViewModel> _exercisesListItemViewModel;
        public IEnumerable<ExercisesListItemViewModel> ExercisesList => _exercisesListItemViewModel;


        private readonly ObservableCollection<MuscleGroup> _muscleGroups;
        public IEnumerable<MuscleGroup> MuscleGroupList => _muscleGroups;

        public ICommand? AddToRoutineCommand { get; }
        public MuscleGroup? SelectedMuscle
        {
            get { return _exercisesDataStore.SelectedMuscle; }
            set { _exercisesDataStore.SelectedMuscle = value; }
        }
        public ExercisesViewModel(ExercisesDataStore exercisesDataStore, DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore, NavigationStore navigationStore, NavigationService<RoutineDetailsViewModel> navigationService, RoutineItemsBufferListViewModel routineItemListViewModel)
        {
            _exercisesDataStore = exercisesDataStore;
            _dayGroupDataStore = dayGroupDataStore;
            _routineItemDataStore = routineItemDataStore;
            _navigationStore = navigationStore;
            _navigationService = navigationService;

            _exercisesListItemViewModel = new ObservableCollection<ExercisesListItemViewModel>();
            _muscleGroups = new ObservableCollection<MuscleGroup>();
            _exercisesDataStore.ExercisesLoaded += _exercisesDataStore_ExercisesLoaded;
            _exercisesDataStore.MuscleGroupsLoaded += _exercisesDataStore_MuscleGroupsLoaded;
            _exercisesDataStore.SelectedMuscleChanged += _exercisesDataStore_SelectedMuscleChanged;
            LoadExercisesCommand = new LoadExercisesCommand(_exercisesDataStore, this);
            RoutineItemListViewModel = routineItemListViewModel;
            //AddToRoutineCommand = new CreateRoutineItemsModelCommand(_dayGroupDataStore, _routineItemDataStore, this, _navigationService);
        }

        private void _exercisesDataStore_SelectedMuscleChanged()
        {

            _exercisesListItemViewModel.Clear();
            foreach (var item in _exercisesDataStore.Exercises.Where(x => x.MuscleGroupId == SelectedMuscle!.Id))
            {
                AddExercise(item);
            }
        }

        public ICommand LoadExercisesCommand { get; set; }
        private void _exercisesDataStore_MuscleGroupsLoaded()
        {

            foreach (var muscle in _exercisesDataStore.MuscleGroups)
            {
                _muscleGroups.Add(muscle);
            }
            SelectedMuscle = _muscleGroups.FirstOrDefault();
        }

        private void _exercisesDataStore_ExercisesLoaded()
        {
            _exercisesListItemViewModel.Clear();
            foreach (var item in _exercisesDataStore.Exercises)
            {
                AddExercise(item);
            }
        }
        private void AddExercise(Exercises exercise)
        {
            ExercisesListItemViewModel exercisesListItemViewModel = new ExercisesListItemViewModel(exercise,_routineItemDataStore,_dayGroupDataStore);
            _exercisesListItemViewModel.Add(exercisesListItemViewModel);
            //_groupLists.SingleOrDefault(x => x.Id == exercise.MuscleGroupId)!.Exercises.Add(exercisesListItemViewModel);
        }

        public static ExercisesViewModel LoadViewModel(ExercisesDataStore exercisesDataStore, DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore, NavigationStore navigationStore, NavigationService<RoutineDetailsViewModel> navigationService, RoutineItemsBufferListViewModel routineItemListViewModel)
        {
            ExercisesViewModel viewModel = new(exercisesDataStore, dayGroupDataStore, routineItemDataStore, navigationStore, navigationService, routineItemListViewModel);

            viewModel.LoadExercisesCommand.Execute(null);

            return viewModel;
        }
    }
}
