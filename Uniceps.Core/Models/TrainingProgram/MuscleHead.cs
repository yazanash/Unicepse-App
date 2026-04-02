using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.TrainingProgram
{
    public class MuscleHead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Code { get; set; } = string.Empty;

        public string MuscleGroupCode { get; set; } = string.Empty;

        [ForeignKey(nameof(MuscleGroupCode))]
        public virtual MuscleGroupV2? Group { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
