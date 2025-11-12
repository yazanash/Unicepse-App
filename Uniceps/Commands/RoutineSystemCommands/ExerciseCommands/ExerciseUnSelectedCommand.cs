using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.ExerciseCommands
{
    public class ExerciseUnSelectedCommand : CommandBase
    {
        private readonly ExercisesListViewModel _viewModel;

        public ExerciseUnSelectedCommand(ExercisesListViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.RemoveTodoItem(_viewModel.UnSelectedExerciseViewModel!);
        }
    }
}
