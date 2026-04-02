using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.API.Models
{
    public class MuscleGroupDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<MuscleHeadResponse> MuscleHeads { get; set; } = new List<MuscleHeadResponse>();

    }
}
