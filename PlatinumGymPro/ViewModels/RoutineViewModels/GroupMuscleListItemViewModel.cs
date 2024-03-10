using PlatinumGym.Core.Models.TrainingProgram;
using PlatinumGymPro.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.RoutineViewModels
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
