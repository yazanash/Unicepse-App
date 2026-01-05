using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.RoutineModels
{
    public class RoutineModel:DomainObject
    {
        public string? Name { get; set; }
        public RoutineLevel Level { get; set; }
        public virtual List<DayGroup> Days { get; set; } = new List<DayGroup>();

    }
    public enum RoutineLevel
    {
        [Display(Name = "غير محدد")]
        None = 0,
        [Display(Name = "مبتدئ")]
        Beginner = 1,
        [Display(Name = "مبتدئ الى متوسط")]
        Novice = 2,
        [Display(Name = "متوسط")]
        Intermediate = 3,
        [Display(Name = "متقدم")]
        Advanced = 4,
        [Display(Name = "محترف")]
        Elite = 5
    }
}
