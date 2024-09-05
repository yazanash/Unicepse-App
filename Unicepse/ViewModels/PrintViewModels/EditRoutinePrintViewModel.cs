﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;
using Unicepse.Stores;
using Unicepse.ViewModels.RoutineViewModels;

namespace Unicepse.ViewModels.PrintViewModels
{
    public class EditRoutinePrintViewModel : ViewModelBase
    {
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly EditSelectRoutineDaysMuscleGroupViewModel _selectRoutineDaysMuscleGroupViewModel;
        public EditRoutinePrintViewModel(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, EditSelectRoutineDaysMuscleGroupViewModel selectRoutineDaysMuscleGroupViewModel)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _routineDataStore.daysItemCreated += _routineDataStore_daysItemCreated;
            _routineDataStore.daysItemDeleted += _routineDataStore_daysItemDeleted;
            _selectRoutineDaysMuscleGroupViewModel = selectRoutineDaysMuscleGroupViewModel;
            _routineExercisesItemsViewModels = new ObservableCollection<RoutineExercisesItemsViewModel>();
            _dayGroupListItemViewModels = new ObservableCollection<DayGroupListItemViewModel>();
            foreach (var routine in _routineDataStore.RoutineItems)
            {
                RoutineExercisesItemsViewModel routineExercisesItemsViewModel = new(routine);
                _routineExercisesItemsViewModels.Add(routineExercisesItemsViewModel);
            }
            foreach (var day in _routineDataStore.DaysItems)
            {
                _dayGroupListItemViewModels.Add(day);
            }
        }
        private void _routineDataStore_daysItemDeleted(DayGroupListItemViewModel obj)
        {
            _dayGroupListItemViewModels.Remove(obj);
        }

        private void _routineDataStore_daysItemCreated(DayGroupListItemViewModel obj)
        {
            _dayGroupListItemViewModels.Add(obj);
        }
        private readonly ObservableCollection<RoutineExercisesItemsViewModel> _routineExercisesItemsViewModels;
        private readonly ObservableCollection<DayGroupListItemViewModel> _dayGroupListItemViewModels;

        public IEnumerable<DayGroupListItemViewModel> DaysGroup => _dayGroupListItemViewModels;

        public IEnumerable<RoutineExercisesItemsViewModel> LegsExercisesList
            => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Legs).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> ChestExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Chest).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> BackExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Back).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> ShouldersExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Shoulders).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> BicepsExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Biceps).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> TricepsExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Triceps).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> CalvesExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Calves).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> AbsExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Abs).OrderBy(x => x.ItemOrder);
        public string? Date => _selectRoutineDaysMuscleGroupViewModel.Date.ToShortDateString();
        public string? Id => _selectRoutineDaysMuscleGroupViewModel.Number;
        public string? FullName => _playersDataStore.SelectedPlayer!.FullName;

        public override void Dispose()
        {
            _routineDataStore.daysItemCreated -= _routineDataStore_daysItemCreated;
            _routineDataStore.daysItemDeleted -= _routineDataStore_daysItemDeleted;
            base.Dispose();
        }
    }
}
