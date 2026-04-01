using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.DataExporter.Dtos
{
    public class RoutineItemDto
    {
        public string ExerciseId { get; set; } = string.Empty;
        public int Order { get; set; }
        public string ExerciseName{ get; set; }="";
        public virtual List<SetDto> Sets { get; set; } = new List<SetDto>();
    }
}
