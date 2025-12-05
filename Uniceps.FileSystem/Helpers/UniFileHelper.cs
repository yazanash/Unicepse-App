using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.FileSystem.Helpers
{
    public class UniFileHelper
    {
        public static string EnsureUniExtension(string filePath)
        {
            return Path.ChangeExtension(filePath, ".unx");
        }
    }
}
