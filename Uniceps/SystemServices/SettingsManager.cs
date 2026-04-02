using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Uniceps.Models;

namespace Uniceps.SystemServices
{
    public static class SettingsManager
    {
        private static string _directoryPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Uniceps"
            );
        private static string _filePath = Path.Combine(_directoryPath, "user.settings");

        public static AppSettings Current { get; private set; } = new AppSettings();
        public static string AppDataPath = _directoryPath;

        static SettingsManager()
        {
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
            Load();
        }
        public static void Save()
        {
            string json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public static void Load()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                Current = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            else
            {
                Current = new AppSettings();
            }
        }
    }
}
