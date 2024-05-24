using Newtonsoft.Json.Linq;
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
            RoutineSchedule = new List<RoutineItems>();
            DaysGroupMap = new Dictionary<int, List<int>>();
        }
        public int Id { get; set; }
        public string? RoutineNo{ get; set;}
        public DateTime RoutineData { get; set; }
        public bool IsTemplate { get; set; }
        public Player.Player? Player { get; set; }
        public List<RoutineItems> RoutineSchedule { get; set; }
        public Dictionary<int, List<int>> DaysGroupMap { get; set; }
    }
}
