using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Uniceps.API.common;

namespace Uniceps.API.Services
{
    public class SyncSettingsManager
    {
        private static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Unicepse",
        "sync_settings.json"
    );

        public static SyncSettings Current { get; private set; } = Load();

        public static SyncSettings Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    return JsonSerializer.Deserialize<SyncSettings>(json) ?? new SyncSettings();
                }
            }
            catch
            {
            }

            return new SyncSettings();
        }

        public static void Save(SyncSettings settings)
        {
            try
            {
                string dir = Path.GetDirectoryName(FilePath)!;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
                Current = settings;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("فشل حفظ إعدادات المزامنة", ex);
            }
        }
    }
}
