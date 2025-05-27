using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands
{
    public class RoutineItemReceivedCommand : CommandBase
    {
        private readonly RoutineItemListViewModel _viewModel;

        public RoutineItemReceivedCommand(RoutineItemListViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.AddTodoItem(_viewModel.IncomingRoutineItemViewModel);
        }
    }
}
