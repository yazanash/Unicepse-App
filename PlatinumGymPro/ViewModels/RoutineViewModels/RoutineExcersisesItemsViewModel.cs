using PlatinumGym.Core.Models.TrainingProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.RoutineViewModels
{
    public class RoutineExcersisesItemsViewModel : ViewModelBase
    {
        public RoutineItems routineItem;

        public RoutineExcersisesItemsViewModel(RoutineItems routineItem)
        {
            this.routineItem = routineItem;
        }

        public string? imageId=> routineItem.Exercise!.ImageId;
        public string? Notes =>routineItem.Notes;
        public string? Orders =>routineItem.Orders;
    }
}
