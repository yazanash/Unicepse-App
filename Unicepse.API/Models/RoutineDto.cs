using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.TrainingProgram;

namespace Unicepse.API.Models
{
    public class RoutineDto
    {
        public string? rid { get; set; }
        public string? pid { get; set; }
        public string? gym_id { get; set; }
        public string? routine_no { get; set; }
        public DateTime routine_date { get; set; }
        public Dictionary<int, string?>? days_group_map { get; set; }
        public List<RoutineItemDto> routine_items { get; set; }

        public RoutineDto()
        {
            routine_items = new List<RoutineItemDto>();
        }

        internal PlayerRoutine ToRoutine()
        {
            PlayerRoutine routine = new PlayerRoutine()
            {
                Id = Convert.ToInt32( rid),
                Player = new Core.Models.Player.Player() { Id = Convert.ToInt32(pid) },
                RoutineNo = routine_no,
                RoutineData = routine_date,
            DaysGroupMap = days_group_map!,
            };
            foreach (var item in routine_items)
            {
                routine.RoutineSchedule.Add(item.ToRoutineItem());
            }
            return routine;
        }
        internal void FromRoutine(PlayerRoutine entity)
        {
            rid = entity.Id.ToString();
            pid = entity.Player!.Id.ToString();
            routine_no = entity.RoutineNo;
            routine_date = entity.RoutineData;
            days_group_map = entity.DaysGroupMap;
            foreach (var item in entity.RoutineSchedule)
            {
                RoutineItemDto routineItemDto = new() ;
                routineItemDto.FromRoutineItem(item);
                routine_items.Add(routineItemDto);

            }
            //check_date = entity.CheckDate.ToString("dd/MM/yyyy");
        }
    }
}
