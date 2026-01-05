using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.FileSystem.Models
{
    public class FileEntry
    {
        public string FileName { get; set; } = "";
        public string DataType { get; set; }="";
        public DateTime Timestamp { get; set; }
    }
}
