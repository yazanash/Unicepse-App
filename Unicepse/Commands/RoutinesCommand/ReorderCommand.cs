using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;

namespace Unicepse.Commands.RoutinesCommand
{
    public class ReorderCommand :CommandBase
    {
        private readonly RoutineDataStore _routineDataStore;

        public ReorderCommand(RoutineDataStore routineDataStore)
        {
            _routineDataStore = routineDataStore;
        }

        public override void Execute(object? parameter)
        {
            _routineDataStore.Reorder();
        }
    }
}
