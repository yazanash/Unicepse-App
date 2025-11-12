using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.FileSystem.Models
{
    public class MetaData
    {
        public string? Source { get; set; }
        public string? SchemaVersion { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? FileId { get; set; }
    }
}