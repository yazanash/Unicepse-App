using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels.RoutineTemplateViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.DayGroupCommands
{
    public class DayGroupRemovedCommand : CommandBase
    {
        private readonly RoutineDayGroupViewModel _viewModel;

        public DayGroupRemovedCommand(RoutineDayGroupViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.RemoveTodoItem(_viewModel.RemovedDayGroupViewModel);
        }
    }
}
