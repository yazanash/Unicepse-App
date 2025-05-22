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
    public class UpdateDayGroupCommand : AsyncCommandBase
    {
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly DayGroupViewModel _dayGroupViewModel;
        public UpdateDayGroupCommand(DayGroupDataStore dayGroupDataStore, DayGroupViewModel dayGroupViewModel)
        {
            _dayGroupDataStore = dayGroupDataStore;
            _dayGroupViewModel = dayGroupViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            DayGroup dayGroup = _dayGroupViewModel.DayGroup!;
            dayGroup.Name = _dayGroupViewModel.Name;
            await _dayGroupDataStore.Update(dayGroup);
        }
    }
}
