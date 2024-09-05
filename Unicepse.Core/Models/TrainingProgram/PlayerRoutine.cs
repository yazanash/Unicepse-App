using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;

namespace Unicepse.Core.Models.TrainingProgram
{
    public class PlayerRoutine
    {
        public PlayerRoutine()
        {
            RoutineSchedule = new List<RoutineItems>();
            DaysGroupMap = new Dictionary<int, string?>();
        }
        public int Id { get; set; }
        public string? RoutineNo{ get; set;}
        public DateTime RoutineData { get; set; }
        public bool IsTemplate { get; set; }
        public Player.Player? Player { get; set; }
        public List<RoutineItems> RoutineSchedule { get; set; }
        public Dictionary<int, string?> DaysGroupMap { get; set; }
        public DataStatus DataStatus { get; set; }
    }
}
