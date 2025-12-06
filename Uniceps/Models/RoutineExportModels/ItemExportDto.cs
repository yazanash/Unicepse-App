using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Models.RoutineExportModels
{
    public class ItemExportDto
    {
        public int Order { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public int ExerciseId { get; set; }
        public string ExerciseUrl { get; set; } = string.Empty;
        public MuscleExportDto Muscle { get; set; } = new();
        public List<SetExportDto> Sets { get; set; } = new();
    }
}
