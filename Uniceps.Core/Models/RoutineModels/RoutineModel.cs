using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.RoutineModels
{
    public class RoutineModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Level { get; set; }
        public virtual List<DayGroup> Days { get; set; } = new List<DayGroup>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
