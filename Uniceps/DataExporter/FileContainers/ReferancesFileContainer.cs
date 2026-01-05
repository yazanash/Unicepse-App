using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.FileContainers
{
    public class ReferancesFileContainer
    {
        public List<SportDto> SportDtos { get; set; } = new();
        public List<EmployeeDto> EmployeeDtos{ get; set; } = new();
    }
}
