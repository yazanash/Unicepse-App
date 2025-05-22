using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.Commands.RoutineSystemCommands.DayGroupCommands
{
    public class DeleteDayGroupCommand : AsyncCommandBase
    {
        private readonly DayGroupDataStore _dataStore;

        public DeleteDayGroupCommand(DayGroupDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            DayGroup dayGroup = _dataStore.SelectedDayGroup!;
            await _dataStore.Delete(dayGroup.Id);
        }
    }
}
