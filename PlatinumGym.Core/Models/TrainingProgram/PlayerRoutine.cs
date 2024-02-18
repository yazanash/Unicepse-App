using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Core.Models.TrainingProgram
{
    public class PlayerRoutine
    {
        public PlayerRoutine()
        {
            RoutineSchedule = new HashSet<RoutineItems>();
        }
        public int Id { get; set; }
        public int RoutineNo{ get; set;}
        public DateTime RoutineData { get; set; }
        public Player.Player? Player { get; set; }
        public ICollection<RoutineItems> RoutineSchedule { get; set; }
    }
}
