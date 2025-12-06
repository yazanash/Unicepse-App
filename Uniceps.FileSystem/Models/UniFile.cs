using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.FileSystem.Models
{
    public class UniFile
    {
        public MetaData? Meta { get; set; } 
        public object? Data { get; set; }
    }
}
