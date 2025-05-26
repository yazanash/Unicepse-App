using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.Core.Models.RoutineModels
{
    public class DayGroup
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int RoutineId { get; set; }
        public int Order { get; set; }
        public RoutineModel? Routine { get; set; }
        public List<RoutineItemModel> RoutineItems { get; set; } = new List<RoutineItemModel>();
    }
}
