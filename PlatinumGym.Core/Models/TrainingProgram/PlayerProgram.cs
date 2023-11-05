using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Core.Models.Sport;

namespace PlatinumGym.Core.Models.TrainingProgram
{
    public class PlayerProgram : DomainObject
    {
        public virtual Sport.Sport? Sport { get; set; }
        public string? Name { get; set; }
    }
}
