using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.TrainingProgram
{
    public class ExerciseV2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ExerciseId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MuscleGroupCode { get; set; } = string.Empty;
        public string MuscleHeadCode { get; set; } = string.Empty;
        public string EquipmentCode { get; set; } = string.Empty;
        public string? MuscleAux1 { get; set; }
        public string? MuscleAux2 { get; set; }
        public string? MuscleAux3 { get; set; }

        [ForeignKey(nameof(MuscleGroupCode))]
        public virtual MuscleGroupV2? MuscleGroupV2 { get; set; }

        [ForeignKey(nameof(MuscleHeadCode))]
        public virtual MuscleHead? MuscleHead { get; set; }

        [ForeignKey(nameof(EquipmentCode))]
        public virtual Equipment? Equipment { get; set; }

        public ExerciseMechanism Mechanism { get; set; }
        public int Version { get; set; } = 0;
        public string? ImagePath { get; set; }
        public bool IsLegacy { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
    public enum ExerciseMechanism 
    {
        [Display(Name = "غير محدد")]
        None = 0,
        [Display(Name = "ثنائي")]
        Bi,
        [Display(Name = "احادي")]
        Uni,
        [Display(Name = "وزن الجسد")]
        Bodyweight,
        [Display(Name = "تبادل")]
        Alternate }
}
