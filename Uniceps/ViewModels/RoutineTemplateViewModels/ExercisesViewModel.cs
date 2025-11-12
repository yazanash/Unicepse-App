using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;
using Uniceps.Commands.RoutineSystemCommands.ExerciseCommands;
using Uniceps.navigation;
using Uniceps.ViewModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.navigation.Stores;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class ExercisesViewModel : ListingViewModelBase
    {
        public ExercisesListViewModel ExercisesListViewModel {get;set;}

        public ExercisesViewModel(ExercisesListViewModel exercisesListViewModel)
        {
            ExercisesListViewModel = exercisesListViewModel;
        }
    }
}
