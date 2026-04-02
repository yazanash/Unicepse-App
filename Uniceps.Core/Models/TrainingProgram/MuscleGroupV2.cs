using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.TrainingProgram
{
    public class MuscleGroupV2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<MuscleHead> Heads { get; set; } = new List<MuscleHead>();
    }
}
