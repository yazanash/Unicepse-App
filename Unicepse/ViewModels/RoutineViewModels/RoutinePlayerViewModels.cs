using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Commands;
using Unicepse.Commands.RoutinesCommand;
using Unicepse.Commands.Player;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.utlis.common;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.navigation.Stores;
using Unicepse.navigation;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class RoutinePlayerViewModels : ListingViewModelBase
    {
        private readonly ObservableCollection<RoutinItemViewModel> _routineItemViewModels;
        private readonly ObservableCollection<RoutineExercisesItemsViewModel> _selectedRoutineItemsViewModels;

        private readonly ObservableCollection<GroupMuscleListItemViewModel> _groupMuscleListItemViewModels;
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationStore _navigationStore;
        public IEnumerable<RoutinItemViewModel> RoutineList => _routineItemViewModels;
        public IEnumerable<RoutineExercisesItemsViewModel> SelectedRoutineItemsList => _selectedRoutineItemsViewModels;
        public IEnumerable<GroupMuscleListItemViewModel> MuscleGroup => _groupMuscleListItemViewModels;
        public ICommand LoadAllRoutines { get; }
        public ICommand AddRoutineCommand { get; }
        public RoutinItemViewModel? SelectedRoutine
        {
            get
            {
                return RoutineList
                    .FirstOrDefault(y => y?.playerRoutine == _routineDataStore.SelectedRoutine);
            }
            set
            {
                _routineDataStore.SelectedRoutine = value?.playerRoutine;
                OnPropertyChanged(nameof(SelectedRoutine));

            }
        }
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
        public RoutinePlayerViewModels(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _navigationStore = navigationStore;
            _groupMuscleListItemViewModels = new ObservableCollection<GroupMuscleListItemViewModel>();
            _routineItemViewModels = new ObservableCollection<RoutinItemViewModel>();
            _selectedRoutineItemsViewModels = new ObservableCollection<RoutineExercisesItemsViewModel>();
            _routineDataStore.Created += _routineDataStore_Created;
            _routineDataStore.Deleted += _routineDataStore_Deleted;
            _routineDataStore.Loaded += _routineDataStore_Loaded;
            _routineDataStore.Updated += _routineDataStore_Updated;
            _routineDataStore.StateChanged += _routineDataStore_StateChanged;
            _routineDataStore.MuscleChanged += _routineDataStore_MuscleChanged;
            LoadAllRoutines = new LoadAllRoutinesCommand(_routineDataStore, this, _playersDataStore);
            //SelectedRoutine = RoutineList.FirstOrDefault();
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
            SelectedMuscle = MuscleGroup.FirstOrDefault();
            AddRoutineCommand = new NavaigateCommand<RoutineTemplatesViewModel>(new NavigationService<RoutineTemplatesViewModel>(_navigationStore, () => LoadAddRoutineViewModel(_playersDataStore, _routineDataStore, _navigationStore, this)));
            
        }

        private void _routineDataStore_MuscleChanged(MuscleGroup? muscle)
        {
            if (_routineDataStore.SelectedRoutine != null)
            {
                _selectedRoutineItemsViewModels.Clear();
                foreach (var routineItem in _routineDataStore.SelectedRoutine!.RoutineSchedule.Where(x => x.Exercises!.GroupId == muscle!.Id).OrderBy(x => x.ItemOrder))
                {
                    AddRoutineItem(routineItem);
                }
            }

        }

        private void AddRoutineItem(RoutineItems routineItem)
        {
            RoutineExercisesItemsViewModel routineExercisesItemsViewModel = new RoutineExercisesItemsViewModel(routineItem);
            _selectedRoutineItemsViewModels.Add(routineExercisesItemsViewModel);
        }

        private RoutineTemplatesViewModel LoadAddRoutineViewModel(PlayersDataStore playerStore, RoutineDataStore routineDataStore, NavigationStore navigationStore, RoutinePlayerViewModels routinePlayerViewModels)
        {
            return RoutineTemplatesViewModel.LoadViewModel(routineDataStore, playerStore, navigationStore, routinePlayerViewModels);
        }
        //private AddRoutineViewModel AddRoutineIViewModel(PlayersDataStore playerStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, NavigationStore navigationStore)
        //{
        //    return AddRoutineViewModel.LoadViewModel(playerStore, routineDataStore, navigationService, navigationStore);
        //}
        private void _routineDataStore_StateChanged(PlayerRoutine? obj)
        {
            _selectedRoutineItemsViewModels.Clear();
            if (obj != null)
                foreach (var routineItem in obj!.RoutineSchedule.Where(x => x.Exercises!.GroupId == _routineDataStore.SelectedMuscle!.Id).OrderBy(x => x.ItemOrder))
                {
                    AddRoutineItem(routineItem);
                }
        }

        private void _routineDataStore_Updated(PlayerRoutine obj)
        {
            RoutinItemViewModel? itemViewModel = _routineItemViewModels.FirstOrDefault(y => y.playerRoutine?.Id == obj.Id);

            if (itemViewModel != null)
            {
                itemViewModel.Update(obj);
            }
        }

        private void _routineDataStore_Loaded()
        {
            _routineItemViewModels.Clear();
            if (_playersDataStore.SelectedPlayer != null)
            {
                foreach (var routine in _routineDataStore.Routines.OrderByDescending(x => x.RoutineData))
                {
                    AddRoutine(routine);

                }
                SelectedRoutine = _routineItemViewModels.FirstOrDefault();
            }
           
        }

        private void _routineDataStore_Deleted(int id)
        {
            RoutinItemViewModel? itemViewModel = _routineItemViewModels.FirstOrDefault(y => y.playerRoutine?.Id == id);

            if (itemViewModel != null)
            {
                _routineItemViewModels.Remove(itemViewModel);
            }
        }

        private void _routineDataStore_Created(PlayerRoutine obj)
        {
            AddRoutine(obj);
        }


        private void AddRoutine(PlayerRoutine routine)
        {
            RoutinItemViewModel viewmodel = new(routine, _playersDataStore, _routineDataStore, _navigationStore, this);
            _routineItemViewModels.Add(viewmodel);
        }
        public override void Dispose()
        {
            _routineDataStore.Created -= _routineDataStore_Created;
            _routineDataStore.Deleted -= _routineDataStore_Deleted;
            _routineDataStore.Loaded -= _routineDataStore_Loaded;
            _routineDataStore.Updated -= _routineDataStore_Updated;
            _routineDataStore.StateChanged -= _routineDataStore_StateChanged;
            _routineDataStore.MuscleChanged -= _routineDataStore_MuscleChanged;
            base.Dispose();
        }
        public static RoutinePlayerViewModels LoadViewModel(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore)
        {
            RoutinePlayerViewModels viewModel = new(routineDataStore, playersDataStore, navigationStore);

            viewModel.LoadAllRoutines.Execute(null);

            return viewModel;
        }
    }
}
