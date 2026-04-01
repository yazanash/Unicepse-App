using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.SystemServices
{
    public static class DatabaseStorageService
    {
        public static string ResolveSQLitePath(IConfiguration configuration)
        {
            string dbName = configuration["DatabaseSettings:SqliteFileName"] ?? "UnicepsV2.db";
            string appName = configuration["DatabaseSettings:AppName"] ?? "Unicepse";

            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                appName);

            string newDbPath = Path.Combine(appDataPath, dbName);
            string oldDbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbName);

            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            if (File.Exists(oldDbPath) && !File.Exists(newDbPath))
            {
                try
                {
                    File.Move(oldDbPath, newDbPath);
                }
                catch { File.Copy(oldDbPath, newDbPath); }
            }

            return $"Data Source={newDbPath}";
        }
    }
}
