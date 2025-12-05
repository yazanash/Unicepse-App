using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
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
            OnPropertyChanged(nameof(SetsString));
        }
        public int Id => RoutineItemModel.Id;
        public string? ExerciseImage => RoutineItemModel.Exercise!.ImagePath;
        public string? ExerciseName => RoutineItemModel.Exercise!.Name;
        public int Order => RoutineItemModel.Order;
        public string? SetsString => string.Join(" × ", RoutineItemModel.Sets.Select(x=>x.Repetition));
        internal void Update(RoutineItemModel obj)
        {
            RoutineItemModel = obj;

            OnPropertyChanged(nameof(SetsString));
            OnPropertyChanged(nameof(ExerciseName));
        }
        internal void UpdateSets(SetModel obj)
        {
            if (RoutineItemModel.Sets.Any(x => x.Id == obj.Id))
            {
                var set = RoutineItemModel.Sets.FirstOrDefault(x => x.Id == obj.Id);
                if (set != null)
                {
                    set.Repetition = obj.Repetition;
                    set.RoundIndex = obj.RoundIndex;
                }
            }
            else
                RoutineItemModel.Sets.Add(obj);

            OnPropertyChanged(nameof(SetsString));
            OnPropertyChanged(nameof(ExerciseName));
        }
        internal void RemoveSet(int obj)
        {
            if (RoutineItemModel.Sets.Any(x => x.Id == obj))
            {
                var set = RoutineItemModel.Sets.FirstOrDefault(x => x.Id == obj);

                if (set != null)
                {
                    RoutineItemModel.Sets.Remove(set);
                }
            }
            OnPropertyChanged(nameof(SetsString));
            OnPropertyChanged(nameof(ExerciseName));
        }
    }
}
