using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Models.RoutineExportModels
{
    public class RoutineExportDto
    {
        public string RoutineName { get; set; } = string.Empty;
        public List<DayExportDto> Days { get; set; } = new();
    }
}
