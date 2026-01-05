using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.FileContainers
{
    public class RoutinesFileContainer
    {
        public List<RoutineDto> RoutineDtos { get; set; } = new List<RoutineDto>();
    }
}
