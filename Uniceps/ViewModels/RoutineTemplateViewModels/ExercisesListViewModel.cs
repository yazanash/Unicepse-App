using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Uniceps.Commands.RoutineSystemCommands.ExerciseCommands;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores.RoutineStores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class ExercisesListViewModel : ListingViewModelBase
    {
        private readonly ExercisesDataStore _exercisesDataStore;
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly RoutineItemDataStore _routineItemDataStore;
        public readonly ObservableCollection<ExercisesListItemViewModel> _exercisesListItemViewModel;
        public ICollectionView ExercisesList { get; }
        private readonly ObservableCollection<MuscleGroup> _muscleGroups;
        public IEnumerable<MuscleGroup> MuscleGroupList => _muscleGroups;

        public ICommand? AddToRoutineCommand { get; }
        public MuscleGroup? SelectedMuscle
        {
            get { return _exercisesDataStore.SelectedMuscle; }
            set { _exercisesDataStore.SelectedMuscle = value; ExercisesList.Refresh(); }
        }
        public int SelectedCount => _exercisesListItemViewModel.Where(x => x.IsSelected).Count();
        public ExercisesListViewModel(ExercisesDataStore exercisesDataStore, DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore)
        {
            _exercisesDataStore = exercisesDataStore;
            _dayGroupDataStore = dayGroupDataStore;
            _routineItemDataStore = routineItemDataStore;
            _exercisesListItemViewModel = new ObservableCollection<ExercisesListItemViewModel>();
                 ExercisesList = CollectionViewSource.GetDefaultView(_exercisesListItemViewModel);
            ExercisesList.Filter = CheckExerciseFilter;
            _muscleGroups = new ObservableCollection<MuscleGroup>();
            _exercisesDataStore.ExercisesLoaded += _exercisesDataStore_ExercisesLoaded;
            _exercisesDataStore.MuscleGroupsLoaded += _exercisesDataStore_MuscleGroupsLoaded;
            LoadExercisesCommand = new LoadExercisesCommand(_exercisesDataStore, this);
            AddToRoutineCommand = new CreateRoutineItemsModelCommand(_dayGroupDataStore, _routineItemDataStore, this);
        }
        public Action? RoutineItemsCreate;
        public void OnRoutineItemsCreated()
        {
            RoutineItemsCreate?.Invoke();
        }
        public void OnSelectedChanged()
        {
            OnPropertyChanged(nameof(SelectedCount));
        }
        private bool CheckExerciseFilter(object obj)
        {
            if (obj is ExercisesListItemViewModel exercisesListItemViewModel)
            {
                bool matchText =
                   SelectedMuscle!=null && exercisesListItemViewModel!.Exercises.MuscleGroupId == SelectedMuscle.PublicId;

                return matchText ;
            }
            return false;
        }

        public void ClearSelection()
        {
            foreach(var item in _exercisesListItemViewModel)
            {
                item.IsSelected = false;
            }
        }
        public void SetChecks()
        {
            foreach (var item in _exercisesListItemViewModel)
            {
                item.IsChecked = _routineItemDataStore.RoutineItems.Any(x => x.ExerciseId == item.Id); ;
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
            ExercisesListItemViewModel exercisesListItemViewModel = new ExercisesListItemViewModel(exercise, _routineItemDataStore, _dayGroupDataStore,this);
            _exercisesListItemViewModel.Add(exercisesListItemViewModel);
            exercisesListItemViewModel.IsChecked = _routineItemDataStore.RoutineItems.Any(x => x.ExerciseId == exercise.Id);


        }
        public static ExercisesListViewModel LoadViewModel(ExercisesDataStore exercisesDataStore, DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore, NavigationStore navigationStore, NavigationService<RoutineDetailsViewModel> navigationService, RoutineItemsBufferListViewModel routineItemListViewModel)
        {
            ExercisesListViewModel viewModel = new(exercisesDataStore, dayGroupDataStore, routineItemDataStore);

            viewModel.LoadExercisesCommand.Execute(null);

            return viewModel;
        }

    }
}
