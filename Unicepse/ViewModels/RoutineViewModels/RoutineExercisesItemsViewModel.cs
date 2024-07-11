using Unicepse.Core.Models.TrainingProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class RoutineExercisesItemsViewModel : ViewModelBase
    {
        public RoutineItems routineItem;

        public RoutineExercisesItemsViewModel(RoutineItems routineItem)
        {
            this.routineItem = routineItem;
        }
        public string? ExerciseName => routineItem.Exercises!.Name;
        public int GroupId => routineItem.Exercises!.GroupId;
        public int ItemOrder => routineItem.ItemOrder;
        public string? imageId => "pack://application:,,,/Resources/Assets/Exercises/" + routineItem.Exercises!.GroupId + "/" + routineItem.Exercises!.ImageId + ".png";
        public string? Notes => routineItem.Notes;
        public string? Orders => routineItem.Orders;
    }
}
