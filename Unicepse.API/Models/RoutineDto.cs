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
        public List<RoutineItemDto> routine_items { get; set; }

        public RoutineDto()
        {
            routine_items = new List<RoutineItemDto>();
        }

        internal PlayerRoutine ToRoutine()
        {
            PlayerRoutine routine = new PlayerRoutine()
            {
                Id = rid,
                Player = new Core.Models.Player.Player() { Id = pid },
                RoutineNo = routine_no,
                RoutineData = Convert.ToDateTime(routine_date),
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
            rid = entity.Id;
            pid = entity.Player!.Id;
            routine_no = entity.RoutineNo;
            routine_date = entity.RoutineData.ToString("dd/MM/yyyy");
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
