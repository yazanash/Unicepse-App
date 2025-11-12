using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.FileSystem.Helpers;
using Uniceps.FileSystem.Models;

namespace Uniceps.FileSystem
{
    public class FileManager
    {
        public static UniFileContainer? Read(string filePath)
        {
            return JsonHelper.Read<UniFileContainer>(filePath);
        }

        public static void Write(UniFileContainer file, string filePath)
        {
            JsonHelper.Write(filePath, file);
        }
    }
}
