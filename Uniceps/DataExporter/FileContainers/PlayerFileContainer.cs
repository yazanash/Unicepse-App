using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.FileContainers
{
    public class PlayerFileContainer
    {
        public List<PlayerDto> PlayerDtos { get; set; } = new();

    }
}
