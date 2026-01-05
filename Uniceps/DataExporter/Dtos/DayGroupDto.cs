using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;

namespace Uniceps.DataExporter.Dtos
{
    public class DayGroupDto
    {
        public string? Name { get; set; }
        public int Order { get; set; }
        public List<RoutineItemDto> RoutineItems { get; set; } = new List<RoutineItemDto>();
    }
}
