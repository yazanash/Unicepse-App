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
        public int ExerciseTId { get; set; }
        public int Order { get; set; }
        public virtual List<SetDto> Sets { get; set; } = new List<SetDto>();
    }
}
