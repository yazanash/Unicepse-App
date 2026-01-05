using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;

namespace Uniceps.DataExporter.Dtos
{
    public class RoutineDto
    {
        public Guid SyncId { get; set; }
        public string? Name { get; set; }
        public RoutineLevel Level { get; set; }
        public virtual List<DayGroupDto> Days { get; set; } = new List<DayGroupDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
