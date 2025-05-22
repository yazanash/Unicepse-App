using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.utlis.common;
using Uniceps.Commands.RoutineSystemCommands.DayGroupCommands;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels
{
    public class RoutineItemListItemViewModel : ViewModelBase
    {
        public RoutineItemModel RoutineItemModel;
        private readonly RoutineItemDataStore _routineItemDataStore;
        public ICommand? DeleteCommand { get; }
       
        public RoutineItemListItemViewModel(RoutineItemModel routineItemModel, RoutineItemDataStore routineItemDataStore)
        {
            RoutineItemModel = routineItemModel;
            _routineItemDataStore = routineItemDataStore;

            DeleteCommand = new DeleteRoutineItemCommand(_routineItemDataStore);
         
        }
        public int Id => RoutineItemModel.Id;
        public string? ExerciseImage => RoutineItemModel.Exercise.ImagePath;
        public string? ExerciseName => RoutineItemModel.Exercise.Name;
        public int Order => RoutineItemModel.Order;
        internal void Update(RoutineItemModel obj)
        {
            RoutineItemModel = obj;
            OnPropertyChanged(nameof(ExerciseName));
        }
    }
}
