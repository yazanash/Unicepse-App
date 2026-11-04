using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.DayGroupCommands
{
    public class CreateDayGroupCommand : AsyncCommandBase
    {
        private readonly DayGroupDataStore _dataStore;
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly RoutineDayGroupViewModel _routineDayGroupViewModel;
        public CreateDayGroupCommand(DayGroupDataStore dataStore, RoutineTempDataStore routineTempDataStore, RoutineDayGroupViewModel routineDayGroupViewModel)
        {
            _dataStore = dataStore;
            _routineTempDataStore = routineTempDataStore;
            _routineDayGroupViewModel = routineDayGroupViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            DayGroup dayGroup = new DayGroup();
            dayGroup.Name = "اليوم " + (_dataStore.DayGroups.Count()+1);
            dayGroup.RoutineId = _routineDayGroupViewModel.RoutineId;
            dayGroup.Order = _dataStore.DayGroups.Count();
            await _dataStore.Add(dayGroup);

        }
    }
}
