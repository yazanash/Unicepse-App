using Unicepse.Core.Models.TrainingProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class GroupMuscleListItemViewModel : ViewModelBase
    {
        public MuscleGroup MuscleGroup;

        public string? Name => MuscleGroup.Name;
        public string? Image => MuscleGroup.Image;


        public GroupMuscleListItemViewModel(MuscleGroup muscleGroup)
        {
            MuscleGroup = muscleGroup;
        }
    }
}
