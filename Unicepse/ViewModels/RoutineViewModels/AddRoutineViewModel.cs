﻿using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Commands;
using Unicepse.Commands.RoutinesCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.utlis.common;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.navigation.Stores;
using Unicepse.navigation;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class AddRoutineViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<ExercisesListItemViewModel> _exercisesListItemViewModel;

        private readonly ObservableCollection<GroupMuscleListItemViewModel> _groupMuscleListItemViewModels;
        private readonly ObservableCollection<RoutineItemFillViewModel> _routineExercisesItemsViewModels;

        private readonly PlayersDataStore _playersDataStore;
        private readonly RoutineDataStore _routineDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly NavigationService<RoutinePlayerViewModels> _navigationService;
        private readonly LicenseDataStore _licenseDataStore;
        public GroupMuscleListItemViewModel? SelectedMuscle
        {
            get
            {
                return MuscleGroup
                    .FirstOrDefault(y => y?.MuscleGroup == _routineDataStore.SelectedMuscle);
            }
            set
            {
                _routineDataStore.SelectedMuscle = value?.MuscleGroup;
                OnPropertyChanged(nameof(SelectedMuscle));

            }
        }
        public IEnumerable<ExercisesListItemViewModel> ExercisesList => _exercisesListItemViewModel;
        public IEnumerable<GroupMuscleListItemViewModel> MuscleGroup => _groupMuscleListItemViewModels;
        public IEnumerable<RoutineItemFillViewModel> RoutineItems => _routineExercisesItemsViewModels;
        public AddRoutineViewModel(PlayersDataStore playersDataStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, NavigationStore navigationStore, RoutineTemplatesViewModel routineTemplatesViewModel, LicenseDataStore licenseDataStore)
        {
            _playersDataStore = playersDataStore;
            _routineDataStore = routineDataStore;
            _navigationService = navigationService;
            _navigationStore = navigationStore;
            _licenseDataStore = licenseDataStore;
            _exercisesListItemViewModel = new ObservableCollection<ExercisesListItemViewModel>();
            _groupMuscleListItemViewModels = new ObservableCollection<GroupMuscleListItemViewModel>();
            _routineExercisesItemsViewModels = new ObservableCollection<RoutineItemFillViewModel>();
            _routineDataStore.ExercisesLoaded += _routineDataStore_ExercisesLoaded;
            LoadExercisesItems = new LoadExercisesCommand(_routineDataStore, this);
            _routineDataStore.MuscleChanged += _routineDataStore_MuscleChanged;
            _routineDataStore.RoutineItemCreated += _routineDataStore_RoutineItemCreated;
            _routineDataStore.RoutineItemDeleted += _routineDataStore_RoutineItemDeleted;
            _routineDataStore.RoutineItemsCleared += _routineDataStore_RoutineItemsCleared;
            _routineDataStore.Reorderd += _routineDataStore_Reorderd;
            _routineDataStore.OrdersApplied += _routineDataStore_OrdersApplied;
            AddMuscleGroups();

            SelectedMuscle = MuscleGroup.FirstOrDefault();
            SubmitCommand = new NavaigateCommand<SelectRoutineDaysMuscleGroupViewModel>(new NavigationService<SelectRoutineDaysMuscleGroupViewModel>(_navigationStore, () => new SelectRoutineDaysMuscleGroupViewModel(_routineDataStore, _navigationService, _playersDataStore, this, _navigationStore,_licenseDataStore)));
            CancelCommand = new NavaigateCommand<RoutineTemplatesViewModel>(new NavigationService<RoutineTemplatesViewModel>(_navigationStore, () => routineTemplatesViewModel));
            ReorderCommand = new ReorderCommand(_routineDataStore);
           
        }

        private void _routineDataStore_OrdersApplied(RoutineItems obj)
        {
            foreach(var item in _routineExercisesItemsViewModels.Where(x=>x.GroupId == obj.Exercises!.GroupId))
            {
                item.Orders = obj.Orders;
            }
        }

        private void _routineDataStore_Reorderd()
        {
            _routineExercisesItemsViewModels.Clear();
            if(SelectedMuscle!=null)
            foreach (var routineItem in _routineDataStore.RoutineItems.Where(x => x.Exercises!.GroupId == SelectedMuscle!.MuscleGroup.Id).OrderBy(x => x.ItemOrder))
            {
                AddRoutineItem(routineItem);
            }
        }

        private void AddMuscleGroups()
        {
            _groupMuscleListItemViewModels.Add(new GroupMuscleListItemViewModel(new MuscleGroup()
            {
                Image = "pack://application:,,,/Resources/Assets/metric/Waist.png",
                FolderName = "Abs",
                Name = "معدة",
                Id = (int)EMuscleGroup.Abs
            }));
            _groupMuscleListItemViewModels.Add(new GroupMuscleListItemViewModel(new MuscleGroup()
            {
                Image = "pack://application:,,,/Resources/Assets/metric/Waist.png",
                FolderName = "Back",
                Name = "ظهر",
                Id = (int)EMuscleGroup.Back
            }));
            _groupMuscleListItemViewModels.Add(new GroupMuscleListItemViewModel(new MuscleGroup()
            {
                Image = "pack://application:,,,/Resources/Assets/metric/ForeArm.png",
                FolderName = "Triceps",
                Name = "ترايسبس",
                Id = (int)EMuscleGroup.Triceps
            }));
            _groupMuscleListItemViewModels.Add(new GroupMuscleListItemViewModel(new MuscleGroup()
            {
                Image = "pack://application:,,,/Resources/Assets/metric/ForeArm.png",
                FolderName = "Biceps",
                Name = "بايسيبس",
                Id = (int)EMuscleGroup.Biceps
            }));
            _groupMuscleListItemViewModels.Add(new GroupMuscleListItemViewModel(new MuscleGroup()
            {
                Image = "pack://application:,,,/Resources/Assets/metric/Shoulder.png",
                FolderName = "Shoulders",
                Name = "الأكتاف",
                Id = (int)EMuscleGroup.Shoulders
            }));
            _groupMuscleListItemViewModels.Add(new GroupMuscleListItemViewModel(new MuscleGroup()
            {
                Image = "pack://application:,,,/Resources/Assets/metric/Thigh.png",
                FolderName = "Legs",
                Name = "الارجل",
                Id = (int)EMuscleGroup.Legs
            }));
            _groupMuscleListItemViewModels.Add(new GroupMuscleListItemViewModel(new MuscleGroup()
            {
                Image = "pack://application:,,,/Resources/Assets/metric/Leg.png",
                FolderName = "Calves",
                Name = "بطات الارجل",
                Id = (int)EMuscleGroup.Calves
            }));
            _groupMuscleListItemViewModels.Add(new GroupMuscleListItemViewModel(new MuscleGroup()
            {
                Image = "pack://application:,,,/Resources/Assets/metric/Chest.png",
                FolderName = "Chest",
                Name = "الصدر",
                Id = (int)EMuscleGroup.Chest
            }));
        }

        public AddRoutineViewModel(PlayersDataStore playersDataStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, NavigationStore navigationStore, bool FromTemp, RoutineTemplatesViewModel routineTemplatesViewModel, LicenseDataStore licenseDataStore)
        {
            _playersDataStore = playersDataStore;
            _routineDataStore = routineDataStore;
            _navigationService = navigationService;
            _navigationStore = navigationStore; 
            _licenseDataStore = licenseDataStore;
            _exercisesListItemViewModel = new ObservableCollection<ExercisesListItemViewModel>();
            _groupMuscleListItemViewModels = new ObservableCollection<GroupMuscleListItemViewModel>();
            _routineExercisesItemsViewModels = new ObservableCollection<RoutineItemFillViewModel>();
            loadRoutineItems();
            _routineDataStore.ExercisesLoaded += _routineDataStore_ExercisesLoaded;
            LoadExercisesItems = new LoadExercisesCommand(_routineDataStore, this);
            _routineDataStore.MuscleChanged += _routineDataStore_MuscleChanged;
            _routineDataStore.RoutineItemCreated += _routineDataStore_RoutineItemCreated;
            _routineDataStore.RoutineItemDeleted += _routineDataStore_RoutineItemDeleted;
            _routineDataStore.RoutineItemsCleared += _routineDataStore_RoutineItemsCleared;
            _routineDataStore.OrdersApplied += _routineDataStore_OrdersApplied;

            AddMuscleGroups();
            SubmitCommand = new NavaigateCommand<SelectRoutineDaysMuscleGroupViewModel>(new NavigationService<SelectRoutineDaysMuscleGroupViewModel>(_navigationStore, () => new SelectRoutineDaysMuscleGroupViewModel(_routineDataStore, _navigationService, _playersDataStore, FromTemp, this, _navigationStore,_licenseDataStore)));
            CancelCommand = new NavaigateCommand<RoutineTemplatesViewModel>(new NavigationService<RoutineTemplatesViewModel>(_navigationStore, () => routineTemplatesViewModel));
            ReorderCommand = new ReorderCommand(_routineDataStore);
           
        }

        private void _routineDataStore_RoutineItemsCleared(List<Exercises> exercises)
        {
            _routineExercisesItemsViewModels.Clear();
            foreach (var exercise in exercises)
                AddExercise(exercise);
        }

        private void loadRoutineItems()
        {
            _routineDataStore.ClearRoutineItems();
            foreach (var item in _routineDataStore.SelectedRoutine!.RoutineSchedule)
            {
                _routineDataStore.AddRoutineItem(item);
            }
        }

        private void _routineDataStore_RoutineItemDeleted(RoutineItems obj)
        {

            _routineExercisesItemsViewModels.Remove(_routineExercisesItemsViewModels.FirstOrDefault(x => x.RoutineItems == obj)!);
            AddExercise(obj.Exercises!);
        }

        private void _routineDataStore_RoutineItemCreated(RoutineItems obj)
        {
            AddRoutineItem(obj);

        }

        private void AddRoutineItem(RoutineItems obj)
        {
            RoutineItemFillViewModel routineExercisesItemsViewModel = new RoutineItemFillViewModel(obj, _playersDataStore, _routineDataStore);
            _routineExercisesItemsViewModels.Add(routineExercisesItemsViewModel);
            _exercisesListItemViewModel.Remove(_exercisesListItemViewModel.Where(x => x.Id == obj.Exercises!.Id).SingleOrDefault()!);
        }

        private void _routineDataStore_MuscleChanged(MuscleGroup? muscle)
        {
            _exercisesListItemViewModel.Clear();
            foreach (var exercise in _routineDataStore.Exercises.Where(x => x.GroupId == muscle!.Id).OrderBy(x => x.Tid))
            {
                AddExercise(exercise);
            }

            _routineExercisesItemsViewModels.Clear();
            foreach (var routineItem in _routineDataStore.RoutineItems.Where(x => x.Exercises!.GroupId == muscle!.Id).OrderBy(x => x.ItemOrder))
            {
                AddRoutineItem(routineItem);
            }

        }
        private void _routineDataStore_ExercisesLoaded()
        {
            if (SelectedMuscle != null)
            {
                _exercisesListItemViewModel.Clear();
                foreach (var exercise in _routineDataStore.Exercises.Where(x => x.GroupId == SelectedMuscle!.MuscleGroup!.Id).OrderBy(x => x.Tid))
                {

                    AddExercise(exercise);
                }

            }
            SelectedMuscle = MuscleGroup!.FirstOrDefault();
        }

        private void AddExercise(Exercises exercise)
        {
            if (!_exercisesListItemViewModel.Where(x => x.Id == exercise.Id).Any())
            {
                ExercisesListItemViewModel exercisesListItemViewModel = new ExercisesListItemViewModel(exercise, _routineDataStore, _playersDataStore);
                _exercisesListItemViewModel.Add(exercisesListItemViewModel);
            }
        }





        public override void Dispose()
        {
            _routineDataStore.ExercisesLoaded -= _routineDataStore_ExercisesLoaded;
            _routineDataStore.MuscleChanged -= _routineDataStore_MuscleChanged;
            _routineDataStore.RoutineItemCreated -= _routineDataStore_RoutineItemCreated;
            _routineDataStore.RoutineItemDeleted -= _routineDataStore_RoutineItemDeleted;
            base.Dispose();
        }




        public static AddRoutineViewModel LoadViewModel(PlayersDataStore playersDataStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, NavigationStore navigationStore, RoutineTemplatesViewModel routineTemplatesViewModel,LicenseDataStore licenseDataStore)
        {
            AddRoutineViewModel viewModel = new(playersDataStore, routineDataStore, navigationService, navigationStore, routineTemplatesViewModel,licenseDataStore);

            viewModel.LoadExercisesItems.Execute(null);
            routineDataStore.ClearRoutineItems();
            routineDataStore.ClearDaysItems();
            return viewModel;
        }
        public static AddRoutineViewModel LoadViewModel(PlayersDataStore playersDataStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, NavigationStore navigationStore, bool FromTemp, RoutineTemplatesViewModel routineTemplatesViewModel,LicenseDataStore licenseDataStore)
        {
            AddRoutineViewModel viewModel = new(playersDataStore, routineDataStore, navigationService, navigationStore, FromTemp, routineTemplatesViewModel,licenseDataStore);

            viewModel.LoadExercisesItems.Execute(null);

            return viewModel;
        }
        public ICommand LoadExercisesItems { get; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ReorderCommand { get; }
    }
}
