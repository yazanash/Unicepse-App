using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.DayGroupCommands
{
    public class DeleteDayGroupCommand : AsyncCommandBase
    {
        private readonly DayGroupDataStore _dataStore;
        private readonly DayGroupViewModel _dayGroupViewModel;

        public DeleteDayGroupCommand(DayGroupDataStore dataStore, DayGroupViewModel dayGroupViewModel)
        {
            _dataStore = dataStore;
            _dayGroupViewModel = dayGroupViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            DayGroup dayGroup = _dayGroupViewModel.DayGroup;
            await _dataStore.Delete(dayGroup.Id);
        }
    }
}
