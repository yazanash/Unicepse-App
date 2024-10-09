using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Stores;

namespace Unicepse.Commands.RoutinesCommand
{
    public class ApplyToAllCommand : CommandBase
    {
        private readonly RoutineDataStore _routineDataStore;
        RoutineItems _routineItems;
        public ApplyToAllCommand( RoutineDataStore routineDataStore, RoutineItems routineItems)
        {
            _routineDataStore = routineDataStore;
            _routineItems = routineItems;
        }

        public override void Execute(object? parameter)
        {
            //int currentIndex = _routineDataStore.RoutineItems.FindIndex(y => y.Id == entity_id);
            _routineDataStore.ApplyToAll(_routineItems);
        }
    }
}
