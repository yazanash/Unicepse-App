using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class TrainingProgram : DomainObject
    {
        public virtual Training? Trainings { get; set; }
        public virtual TrainingCategory? Category { get; set; }
        public int Counter { get; set; }
        public int Rounds { get; set; }
        public virtual PlayerProgram? PlayerProgram { get; set; }
    }
}
