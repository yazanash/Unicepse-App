using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.API.Models
{
    public class MuscleHeadResponse
    {
        public string Code {  get; set; } =string.Empty;
        public string MuscleGroupCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
