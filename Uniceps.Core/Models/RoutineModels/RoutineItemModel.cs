using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.Core.Models.RoutineModels
{
    public class RoutineItemModel
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public string? ExerciseV2Id { get; set; } = string.Empty;
        [ForeignKey(nameof(ExerciseV2Id))]
        public virtual ExerciseV2? Exercise { get; set; }
        public int DayId { get; set; }
        public DayGroup? Day { get; set; }
        public int Order { get; set; }
        public virtual List<SetModel> Sets { get; set; } = new List<SetModel>();
    }
}
