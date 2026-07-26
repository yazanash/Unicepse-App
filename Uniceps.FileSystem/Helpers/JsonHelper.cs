using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.FileSystem.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true 
        };
        public static T? Read<T>(string filePath)
        {
            if (!File.Exists(filePath)) return default;

            var json = File.ReadAllText(filePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<T>(json, _options);
        }

        public static void Write<T>(string filePath, T obj)
        {
            var json = JsonSerializer.Serialize(obj, _options);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }
    }
}
