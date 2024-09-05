using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.RoutinesCommand;
using Unicepse.Stores;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class DayGroupListItemViewModel : ViewModelBase
    {
        private readonly RoutineDataStore _routineDataStore;
        public int SelectedDay { get; set; }
        public string? Groups { get; set; }
        public DayGroupListItemViewModel(int day, RoutineDataStore routineDataStore)
        {
            SelectedDay = day;
            _routineDataStore = routineDataStore;
            RemoveDaysCommand = new RemoveDaysCommand(_routineDataStore, this);
        }
        public ICommand RemoveDaysCommand { get; }
        public string? GetGroups()
        {
            return Groups;
        }
    }
}
