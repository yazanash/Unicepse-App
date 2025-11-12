using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.TrainingProgram
{
    public class MuscleGroup
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EngName { get; set; }
        public int PublicId { get; set; }
    }
}
