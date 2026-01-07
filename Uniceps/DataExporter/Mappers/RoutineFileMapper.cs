using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.Mappers
{
    public static class RoutineFileMapper
    {
        public static RoutineModel FromDto(RoutineDto data)
        {
            RoutineModel routine = new RoutineModel()
            {
                CreatedAt = data.CreatedAt,
                Level = data.Level,
                SyncId = data.SyncId,
                Name = data.Name,
                UpdatedAt = data.UpdatedAt,
                Days = data.Days.Select(d =>
            new DayGroup
            {
                Name = d.Name,
                Order = d.Order,
                RoutineItems = d.RoutineItems.Select(x => new RoutineItemModel
                {
                    Exercise = new Core.Models.TrainingProgram.Exercises { Tid = x.ExerciseTId ,Name = x.ExerciseName ?? "" },
                    Order = x.Order,
                    Sets = x.Sets.Select(s => new SetModel
                    {
                        Repetition = s.Repetition,
                        RoundIndex = s.RoundIndex
                    }).ToList()
                }).ToList()
            }).ToList()
            };

            return routine;
        }

        public static RoutineDto ToDto(RoutineModel data)
        {
            RoutineDto routineDto = new RoutineDto()
            {
                CreatedAt = data.CreatedAt,
                Level = data.Level,
                Name = data.Name,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId
            };
            routineDto.Days = data.Days.Select(d =>
            new DayGroupDto
            {
                Name = d.Name,
                Order = d.Order,
                // نبدأ هنا بتعبئة قائمة العناصر
                RoutineItems = d.RoutineItems.Select(x => new RoutineItemDto
                {
                    ExerciseTId = x.Exercise?.Tid ?? 0,
                    ExerciseName = x.Exercise?.Name??"",
                    Order = x.Order,
                    Sets = x.Sets.Select(s => new SetDto
                    {
                        Repetition = s.Repetition,
                        RoundIndex = s.RoundIndex
                    }).ToList()
                }).ToList()
            }).ToList();

            return routineDto;
        }
    }
}
