using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels.RoutineViewModels;

namespace Unicepse.Commands.RoutinesCommand
{
    public class RemoveDaysCommand : CommandBase
    {
        private readonly RoutineDataStore _routineDataStore;
        DayGroupListItemViewModel _dayGroupListItemViewModel;
        public RemoveDaysCommand(RoutineDataStore routineDataStore,DayGroupListItemViewModel dayGroupListItemViewModel)
        {
            _routineDataStore = routineDataStore;
            _dayGroupListItemViewModel = dayGroupListItemViewModel;
            _routineDataStore.daysItemCreated += _routineDataStore_daysItemCreated;
            _routineDataStore.daysItemDeleted += _routineDataStore_daysItemDeleted;
        }

        private void _routineDataStore_daysItemDeleted(DayGroupListItemViewModel obj)
        {
            OnCanExecutedChanged();
        }

        private void _routineDataStore_daysItemCreated(DayGroupListItemViewModel obj)
        {
            OnCanExecutedChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return _dayGroupListItemViewModel.SelectedDay== _routineDataStore.DaysItems.Count() && base.CanExecute(null);
        }
        public override void Execute(object? parameter)
        {
            _routineDataStore.DeleteDaysItem(_dayGroupListItemViewModel);
        }
        
    }
}
