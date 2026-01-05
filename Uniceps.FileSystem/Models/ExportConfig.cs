using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.FileSystem.Models
{
    public class ExportConfig
    {
        public string AppVersion { get; set; } = "1.0.0";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<FileEntry> ExportedFiles { get; set; } = new();
        public string SystemInfo { get; set; } = "Uniceps System";
    }
}
