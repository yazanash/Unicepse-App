using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.DayGroupCommands
{
    public class SaveReorderCommand : AsyncCommandBase
    {
        private readonly DayGroupDataStore _dataStore;
        private readonly RoutineDayGroupViewModel _routineDayGroupViewModel;

        public SaveReorderCommand(DayGroupDataStore dataStore, RoutineDayGroupViewModel routineDayGroupViewModel)
        {
            _dataStore = dataStore;
            _routineDayGroupViewModel = routineDayGroupViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            List<DayGroup>? reorderedList = _routineDayGroupViewModel.DayGroups.Select(x => x.DayGroup!).ToList();
           foreach (DayGroup? dayGroup in reorderedList)
            {
                if (dayGroup != null)
                {
                    dayGroup.Order = reorderedList.IndexOf(dayGroup) + 1;
                }
            }
            await _dataStore.UpdateRange(reorderedList);
            _routineDayGroupViewModel.HasOrderChanged = false;
        }
    }
}
