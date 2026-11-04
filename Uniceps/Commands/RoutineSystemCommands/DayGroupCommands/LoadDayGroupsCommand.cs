using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.Commands.RoutineSystemCommands.DayGroupCommands
{
    public class LoadDayGroupsCommand : AsyncCommandBase
    {
        private readonly DayGroupDataStore _dayGroupDataStore;

        public LoadDayGroupsCommand(DayGroupDataStore dayGroupDataStore)
        {
            _dayGroupDataStore = dayGroupDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is int routineId)
                await _dayGroupDataStore.GetAllById(routineId);
        }
    }
}
