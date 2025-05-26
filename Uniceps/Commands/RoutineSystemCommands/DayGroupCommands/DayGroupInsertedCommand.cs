using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels.RoutineTemplateViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.DayGroupCommands
{
    public class DayGroupInsertedCommand : CommandBase
    {
        private readonly RoutineDayGroupViewModel _viewModel;

        public DayGroupInsertedCommand(RoutineDayGroupViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.InsertTodoItem(_viewModel.InsertedDayGroupViewModel, _viewModel.TargetDayGroupViewModel);
        }
    }
}
