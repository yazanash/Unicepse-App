using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.TrainingProgram;

namespace Unicepse.API.Models
{
    public class RoutineDto
    {
        public int rid { get; set; }
        public int pid { get; set; }
        public string? gym_id { get; set; }
        public string? routine_no { get; set; }
        public string? routine_date { get; set; }
        public Dictionary<int, string?>? days_group_map { get; set; }
        public List<RoutineItems>? routine_items { get; set; }

        internal PlayerRoutine ToRoutine()
        {
            PlayerRoutine routine = new PlayerRoutine()
            {
                Id = rid,
                Player = new Core.Models.Player.Player() { Id = pid },
                RoutineNo = routine_no,
                RoutineData =Convert.ToDateTime( routine_date),
                DaysGroupMap = days_group_map!,
                RoutineSchedule = routine_items!
            };
            return routine;
        }
        internal void FromRoutine(PlayerRoutine entity)
        {
            rid = entity.Id;
            pid = entity.Player!.Id;
            routine_no = entity.RoutineNo;
            routine_date = entity.RoutineData.ToString("dd/MM/yyyy");
            days_group_map = entity.DaysGroupMap;
            routine_items = entity.RoutineSchedule;
        //check_date = entity.CheckDate.ToString("dd/MM/yyyy");
    }
    }
}
