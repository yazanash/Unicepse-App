using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.FileContainers
{
    public class PlayerReferancesFileContainer
    {
        public List<MetricDto> MetricDtos { get; set; } = new();
        public List<AttendancesDto> AttendancesDtos { get; set; } = new();
    }
}
