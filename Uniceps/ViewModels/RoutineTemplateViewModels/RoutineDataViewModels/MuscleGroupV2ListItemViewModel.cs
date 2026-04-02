using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels
{
    public class MuscleGroupV2ListItemViewModel : ViewModelBase
    {
        public MuscleGroupV2 MuscleGroupV2;

        public MuscleGroupV2ListItemViewModel(MuscleGroupV2 muscleGroupV2)
        {
            MuscleGroupV2 = muscleGroupV2;
        }
        public string Code => MuscleGroupV2.Code;
        public string Name => MuscleGroupV2.Name;
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }
    }
}
