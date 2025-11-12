using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.ExerciseCommands
{
    public class ExerciseSelectedCommand:CommandBase
    {
        private readonly ExercisesListViewModel _viewModel;

        public ExerciseSelectedCommand(ExercisesListViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.AddTodoItem(_viewModel.SelectedExerciseViewModel!);
        }
    }
}
