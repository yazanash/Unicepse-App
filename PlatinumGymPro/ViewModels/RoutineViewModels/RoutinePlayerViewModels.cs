using PlatinumGym.Core.Models.TrainingProgram;
using PlatinumGymPro.Commands.RoutinesCommand;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.RoutineViewModels
{
    public class RoutinePlayerViewModels : ListingViewModelBase
    {
        private readonly ObservableCollection<RoutinItemViewModel> _routineItemViewModels;
        private readonly ObservableCollection<RoutineExcersisesItemsViewModel> _selectedRoutineItemsViewModels;
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationStore _navigationStore;
        public IEnumerable<RoutinItemViewModel> RoutineList => _routineItemViewModels;
        public IEnumerable<RoutineExcersisesItemsViewModel> SelectedRoutineItemsList => _selectedRoutineItemsViewModels;

        public ICommand LoadAllRoutines { get; }
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
        public RoutinePlayerViewModels(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _navigationStore = navigationStore;

            _routineItemViewModels = new ObservableCollection<RoutinItemViewModel>();
            _selectedRoutineItemsViewModels = new ObservableCollection<RoutineExcersisesItemsViewModel>();
            _routineDataStore.Created += _routineDataStore_Created;
            _routineDataStore.Deleted += _routineDataStore_Deleted;
            _routineDataStore.Loaded += _routineDataStore_Loaded;
            _routineDataStore.Updated += _routineDataStore_Updated;
            _routineDataStore.StateChanged += _routineDataStore_StateChanged;
            LoadAllRoutines = new LoadAllRoutinesCommand(_routineDataStore, this, _playersDataStore);
        }

        private void _routineDataStore_StateChanged(PlayerRoutine? obj)
        {
            _selectedRoutineItemsViewModels.Clear();
          foreach (var routineItem in obj!.RoutineSchedule)
            {
                RoutineExcersisesItemsViewModel routineItemview = new(routineItem);
                _selectedRoutineItemsViewModels.Add(routineItemview);
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
            foreach (var routine in _routineDataStore.Routines)
            {
                AddRoutine(routine);

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
            RoutinItemViewModel viewmodel = new(routine);
            _routineItemViewModels.Add(viewmodel);
        }

        public static RoutinePlayerViewModels LoadViewModel(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore)
        {
            RoutinePlayerViewModels viewModel = new( routineDataStore, playersDataStore,navigationStore);

            viewModel.LoadAllRoutines.Execute(null);

            return viewModel;
        }
    }
}
