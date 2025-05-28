using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands
{
    public class RoutineItemRemovedCommand : CommandBase
    {
        private readonly RoutineItemListViewModel _viewModel;

        public RoutineItemRemovedCommand(RoutineItemListViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.RemoveTodoItem(_viewModel.RemovedRoutineItemViewModel!);
        }
    }
}
