using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.API.Models
{
    public class ExerciseDtoModel
    {
        public string ExerciseId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Implementation { get; set; } = string.Empty;
        public string MuscleGroupCode { get; set; } = string.Empty;
        public string MuscleHeadCode { get; set; } = string.Empty;
        public string EquipmentCode { get; set; } = string.Empty;
        public string? MuscleAux1 { get; set; } = string.Empty;
        public string? MuscleAux2 { get; set; } = string.Empty;
        public string? MuscleAux3 { get; set; } = string.Empty;
        public string? Implemetation { get; set; } = string.Empty;
        public string? Mechanism { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Version { get; set; } = 1;
        public int MediaVersion { get; set; } = 1;
        public DateTime LastUpdated { get; set; } 

    }
}
