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
using Uniceps.Commands;
using Uniceps.Commands.RoutineSystemCommands.ExerciseCommands;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;
using Uniceps.Core.Models.RoutineModels;
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
        private readonly ObservableCollection<MuscleGroupV2ListItemViewModel> _muscleGroups;
        public IEnumerable<MuscleGroupV2ListItemViewModel> MuscleGroupList => _muscleGroups;

        private readonly ObservableCollection<EquipmentListItemViewModel> _equipmentList;
        public IEnumerable<EquipmentListItemViewModel> EquipmentsList => _equipmentList;
        public ObservableCollection<ExerciseMechanism> MechanismList { get; set; } = new();

        public ICommand? AddToRoutineCommand { get; }

        public ICommand? ClearFiltersCommand => new RelayCommand(ClearFilters);

        private ExerciseMechanism _selectedMechanism;
        public ExerciseMechanism SelectedMechanism
        {
            get { return _selectedMechanism; }
            set
            {
                _selectedMechanism = value; ExercisesList.Refresh();
            }
        }
        public int SelectedCount => _exercisesListItemViewModel.Where(x => x.IsSelected).Count();
        public int Total => ExercisesList.Cast<object>().Count();
        public ExercisesListViewModel(ExercisesDataStore exercisesDataStore, DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore)
        {
            _exercisesDataStore = exercisesDataStore;
            _dayGroupDataStore = dayGroupDataStore;
            _routineItemDataStore = routineItemDataStore;
            _exercisesListItemViewModel = new ObservableCollection<ExercisesListItemViewModel>();
            ExercisesList = CollectionViewSource.GetDefaultView(_exercisesListItemViewModel);
            ExercisesList.Filter = CheckExerciseFilter;
            ExercisesList.CollectionChanged += ExercisesList_CollectionChanged;
            _muscleGroups = new ObservableCollection<MuscleGroupV2ListItemViewModel>();
            _equipmentList = new ObservableCollection<EquipmentListItemViewModel>();
            _exercisesDataStore.ExercisesLoaded += _exercisesDataStore_ExercisesLoaded;
            _exercisesDataStore.MuscleGroupsLoaded += _exercisesDataStore_MuscleGroupsLoaded;
            LoadExercisesCommand = new LoadExercisesCommand(_exercisesDataStore, this);
            AddToRoutineCommand = new CreateRoutineItemsModelCommand(_dayGroupDataStore, _routineItemDataStore, this);
            foreach (var item in Enum.GetValues(typeof(ExerciseMechanism)))
            {
                MechanismList.Add((ExerciseMechanism)item);
            }
        }

        private void ExercisesList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Total));
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
            if (obj is ExercisesListItemViewModel item)
            {
                var selectedMuscleCodes = MuscleGroupList.Where(m => m.IsSelected).Select(m => m.Code).ToList();
                var selectedEquipCodes = EquipmentsList.Where(e => e.IsSelected).Select(e => e.Code).ToList();

                bool matchMuscle = !selectedMuscleCodes.Any() ||
                           selectedMuscleCodes.Any(m => m == item.Exercises.MuscleGroupCode);

                bool matchEquip = !selectedEquipCodes.Any() ||
                                  selectedEquipCodes.Any(e => e == item.Exercises.EquipmentCode);

                bool matchMechanism = SelectedMechanism == ExerciseMechanism.None ||
                                      item.Exercises.Mechanism == SelectedMechanism;

                return matchMuscle && matchEquip && matchMechanism;

            }
            return false;
        }

        public void ClearSelection()
        {
            foreach (var item in _exercisesListItemViewModel)
            {
                item.IsSelected = false;
            }
        }
        public void ClearFilters()
        {
            foreach (var item in _muscleGroups)
            {
                item.IsSelected = false;
            }
            foreach (var item in _equipmentList)
            {
                item.IsSelected = false;
            }
        }

        public void SetChecks()
        {
            foreach (var item in _exercisesListItemViewModel)
            {
                item.IsChecked = _routineItemDataStore.RoutineItems.Any(x => x.ExerciseV2Id == item.Id); ;
            }
        }
        public ICommand LoadExercisesCommand { get; set; }
        private void _exercisesDataStore_MuscleGroupsLoaded()
        {
            _muscleGroups.Clear();
            _equipmentList.Clear();
            foreach (var muscle in _exercisesDataStore.MuscleGroups)
            {
                MuscleGroupV2ListItemViewModel muscleItem = new MuscleGroupV2ListItemViewModel(muscle);
                muscleItem.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(MuscleGroupV2ListItemViewModel.IsSelected))
                        ExercisesList.Refresh();
                };
                _muscleGroups.Add(muscleItem);
            }

            foreach (var equip in _exercisesDataStore.Equipments)
            {
                EquipmentListItemViewModel equipItem = new EquipmentListItemViewModel(equip);
                equipItem.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(EquipmentListItemViewModel.IsSelected))
                        ExercisesList.Refresh();
                };
                _equipmentList.Add(equipItem);
            }
        }

        private void _exercisesDataStore_ExercisesLoaded()
        {
            _exercisesListItemViewModel.Clear();
            foreach (var item in _exercisesDataStore.Exercises)
            {
                AddExercise(item);
            }
        }
        private void AddExercise(ExerciseV2 exercise)
        {
            ExercisesListItemViewModel exercisesListItemViewModel = new ExercisesListItemViewModel(exercise, _routineItemDataStore, _dayGroupDataStore, this);
            _exercisesListItemViewModel.Add(exercisesListItemViewModel);
            exercisesListItemViewModel.IsChecked = _routineItemDataStore.RoutineItems.Any(x => x.ExerciseV2Id == exercise.ExerciseId);


        }
        public static ExercisesListViewModel LoadViewModel(ExercisesDataStore exercisesDataStore, DayGroupDataStore dayGroupDataStore, RoutineItemDataStore routineItemDataStore, NavigationStore navigationStore, NavigationService<RoutineDetailsViewModel> navigationService, RoutineItemsBufferListViewModel routineItemListViewModel)
        {
            ExercisesListViewModel viewModel = new(exercisesDataStore, dayGroupDataStore, routineItemDataStore);

            viewModel.LoadExercisesCommand.Execute(null);

            return viewModel;
        }

    }
}
