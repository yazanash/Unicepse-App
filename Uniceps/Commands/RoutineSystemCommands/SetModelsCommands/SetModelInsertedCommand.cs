using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.SetModelsCommands
{
    public class SetModelInsertedCommand : CommandBase
    {
        private readonly SetModelItemsListViewModel _viewModel;

        public SetModelInsertedCommand(SetModelItemsListViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.InsertTodoItem(_viewModel.InsertedSetModelItemViewModel!, _viewModel.TargetSetModelItemViewModel!);
        }
    }
}
