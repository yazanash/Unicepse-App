using PlatinumGym.Core.Models.TrainingProgram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.RoutineViewModels
{
    public class RoutinItemViewModel : ViewModelBase
    {
        public PlayerRoutine playerRoutine { get; set; }
        public RoutinItemViewModel(PlayerRoutine playerRoutine)
        {
            this.playerRoutine = playerRoutine;
        }

        public int Id => playerRoutine.Id;
        public int RoutineNo => playerRoutine.RoutineNo;
        public DateTime routineDate => playerRoutine.RoutineData;

        public void Update(PlayerRoutine obj)
        {
            this.playerRoutine = obj;
        }
    }
}
