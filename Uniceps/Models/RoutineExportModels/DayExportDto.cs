using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Models.RoutineExportModels
{
    public class DayExportDto
    {
        public int Order { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ItemExportDto> Items { get; set; } = new();
    }
}
