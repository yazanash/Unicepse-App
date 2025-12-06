using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Models.RoutineExportModels;
using Uniceps.ViewModels.PrintRoutineViewModel;

namespace Uniceps.Helpers.Mappers
{
    public static class RoutineMapper
    {
        public static RoutineExportDto Map(RoutineModel routine)
        {
            return new RoutineExportDto
            {
                RoutineName = routine.Name ?? string.Empty,
                Days = routine.Days.Select(d => new DayExportDto
                {
                    Order = d.Order,
                    Name = d.Name ?? string.Empty,
                    Items = d.RoutineItems.Select(i => new ItemExportDto
                    {
                        Order = i.Order,
                        ExerciseName = i.Exercise?.Name ?? string.Empty,
                        ExerciseId = i.Exercise?.Tid??0,
                        ExerciseUrl = i.Exercise?.ImageUrl ?? string.Empty,
                        Muscle = new MuscleExportDto
                        {
                            Ar = i.Exercise?.MuscelAr ?? string.Empty,
                            En = i.Exercise?.MuscelEng ?? string.Empty
                        },
                        Sets = i.Sets.Select(s => new SetExportDto
                        {
                            Order = s.RoundIndex,
                            Repetition = s.Repetition
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
        }
        public static PlayerRoutinePrintViewModel MapToPdf(RoutineModel dto, string playerName)
        {
            return new PlayerRoutinePrintViewModel
            {
                GymName = "Uniceps",
                FullName = playerName,
                RoutineName = dto.Name??"اسم البرنامج",
                Date = DateTime.Now.ToString("yyyy/MM/dd"),
                Days = dto.Days
                    .OrderBy(d => d.Order)
                    .Select(d => new DayPrintVm
                    {
                        Order = d.Order,
                        Name = d.Name ?? "اسم اليوم",
                        Items = d.RoutineItems
                            .OrderBy(i => i.Order)
                            .Select(i => new ItemPrintVm
                            {
                                Order = i.Order,
                                ExerciseName = i.Exercise?.Name??"اسم التمرين",
                                ExerciseId = i.ExerciseId,
                                ExerciseUrl = i.Exercise?.ImagePath??"",
                                Muscle = i.Exercise?.MuscelAr??"اسم العضلة",
                                Sets = i.Sets
                                    .OrderBy(s => s.RoundIndex)
                                    .Select(s => new SetPrintVm { Order = s.RoundIndex, Repetition = s.Repetition })
                                    .ToList()
                            }).ToList()
                    }).ToList()
            };
        }
    }
}
