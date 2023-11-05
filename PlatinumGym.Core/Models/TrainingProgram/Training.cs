using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Core.Models.TrainingProgram;
using PlatinumGym.Core.Models.Sport; 

namespace PlatinumGym.Core.Models.TrainingProgram
{
    public class Training: DomainObject
    {
        public string? Name { get; set; }
        public virtual TrainingCategory? Category { get; set; }
        public string? Muscle { get; set; }
        public virtual Sport.Sport? Sport { get; set; }
    }
}
