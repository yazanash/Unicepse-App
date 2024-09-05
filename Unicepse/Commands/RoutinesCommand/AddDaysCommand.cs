using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels.RoutineViewModels;

namespace Unicepse.Commands.RoutinesCommand
{
    public class AddDaysCommand : CommandBase
    {
        private readonly RoutineDataStore _routineDataStore;
        public AddDaysCommand(RoutineDataStore routineDataStore)
        {
            _routineDataStore = routineDataStore;
        }

        public override void Execute(object? parameter)
        {
            int day = _routineDataStore.DaysItems.Count() + 1;
            DayGroupListItemViewModel dayItem= new (day,_routineDataStore);
            _routineDataStore.AddDaysItem(dayItem);
        }

    }
}
