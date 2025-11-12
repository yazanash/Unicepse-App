using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.RoutineSystemCommands.ExerciseCommands;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.navigation.Stores;
using Uniceps.navigation;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class ExercisesListViewModel : ListingViewModelBase
    {
        private readonly ExercisesDataStore _exercisesDataStore;
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly RoutineItemDataStore _routineItemDataStore;
        private readonly NavigationService<RoutineDetailsViewModel> _navigationService;
        private readonly NavigationStore _navigationStore;
        public RoutineItemsBufferListViewModel RoutineItemListViewModel { get; set; }
        private readonly ObservableCollection<ExercisesListItemViewModel> _exercisesListItemViewModel;
        public IEnumerable<ExercisesListItemViewModel> ExercisesList => _exercisesListItemViewModel;


        public readonly ObservableCollection<Exercises> ExercisesBufferListItemViewModel;

        private readonly ObservableCollection<MuscleGroup> _muscleGroups;
        public IEnumerable<MuscleGroup> MuscleGroupList => _muscleGroups;

        public ICommand? AddToRoutineCommand { get; }
        public MuscleGroup? SelectedMuscle
        {
            get { return _exercisesDataStore.SelectedMuscle; }
            set { _exercisesDataStore.SelectedMuscle = value; }
        }



        private ExercisesListItemViewModel? _selectedExerciseViewModel;
        public ExercisesListItemViewModel? SelectedExerciseViewModel
        {
            get
            {
                return _selectedExerciseViewModel;
            }
            set
            {
                _selectedExerciseViewModel = value;
                OnPropertyChanged(nameof(SelectedExerciseViewModel));
            }
        }

        private ExercisesListItemViewModel? _unSelectedExerciseViewModel;
        public ExercisesListItemViewModel? UnSelectedExerciseViewModel
        {
            get
            {
                return _unSelectedExerciseViewModel;
            }
            set
            {
                _unSelectedExerciseViewModel = value;
                OnPropertyChanged(nameof(UnSelectedExerciseViewModel));
            }
        }
        public ICommand ExerciseSelectedCommand { get; }
        public ICommand ExerciseUnSelectedCommand { get; }

        public ExercisesListViewModel(ExercisesDataStore exercisesDataStore, DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore, NavigationStore navigationStore, NavigationService<RoutineDetailsViewModel> navigationService, RoutineItemsBufferListViewModel routineItemListViewModel)
        {
            _exercisesDataStore = exercisesDataStore;
            _dayGroupDataStore = dayGroupDataStore;
            _routineItemDataStore = routineItemDataStore;
            _navigationStore = navigationStore;
            _navigationService = navigationService;
            ExercisesBufferListItemViewModel = new();
            ExerciseSelectedCommand = new ExerciseSelectedCommand(this);
            ExerciseUnSelectedCommand = new ExerciseUnSelectedCommand(this);
            _exercisesListItemViewModel = new ObservableCollection<ExercisesListItemViewModel>();
            _muscleGroups = new ObservableCollection<MuscleGroup>();
            _exercisesDataStore.ExercisesLoaded += _exercisesDataStore_ExercisesLoaded;
            _exercisesDataStore.MuscleGroupsLoaded += _exercisesDataStore_MuscleGroupsLoaded;
            _exercisesDataStore.SelectedMuscleChanged += _exercisesDataStore_SelectedMuscleChanged;
            LoadExercisesCommand = new LoadExercisesCommand(_exercisesDataStore, this);
            RoutineItemListViewModel = routineItemListViewModel;
            AddToRoutineCommand = new CreateRoutineItemsModelCommand(_dayGroupDataStore, _routineItemDataStore, this, _navigationService);
        }

        private void _exercisesDataStore_SelectedMuscleChanged()
        {

            _exercisesListItemViewModel.Clear();
            foreach (var item in _exercisesDataStore.Exercises.Where(x => x.MuscleGroupId == SelectedMuscle!.PublicId))
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
            ExercisesListItemViewModel exercisesListItemViewModel = new ExercisesListItemViewModel(exercise, _routineItemDataStore, _dayGroupDataStore);
            _exercisesListItemViewModel.Add(exercisesListItemViewModel);
            exercisesListItemViewModel.IsChecked = _routineItemDataStore.RoutineItems.Any(x => x.ExerciseId == exercise.Id);
            exercisesListItemViewModel.IsSelected = ExercisesBufferListItemViewModel.Any(x=>x.Id==exercise.Id);


        }
        public static ExercisesListViewModel LoadViewModel(ExercisesDataStore exercisesDataStore, DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore, NavigationStore navigationStore, NavigationService<RoutineDetailsViewModel> navigationService, RoutineItemsBufferListViewModel routineItemListViewModel)
        {
            ExercisesListViewModel viewModel = new(exercisesDataStore, dayGroupDataStore, routineItemDataStore, navigationStore, navigationService, routineItemListViewModel);

            viewModel.LoadExercisesCommand.Execute(null);

            return viewModel;
        }
        public void AddTodoItem(ExercisesListItemViewModel item)
        {
            if (!ExercisesBufferListItemViewModel.Contains(item.Exercises))
            {
                ExercisesBufferListItemViewModel.Add(item.Exercises);
            }
        }
        public void RemoveTodoItem(ExercisesListItemViewModel item)
        {
            ExercisesBufferListItemViewModel.Remove(item.Exercises);
        }

    }
}
