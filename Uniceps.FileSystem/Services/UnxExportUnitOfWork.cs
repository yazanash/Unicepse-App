using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.FileSystem.Helpers;
using Uniceps.FileSystem.Models;

namespace Uniceps.FileSystem.Services
{
    public class UnxExportUnitOfWork
    {
        private readonly string _tempPath;
        private readonly ExportConfig _config;
        public UnxExportUnitOfWork()
        {
            // إنشاء مجلد مؤقت فريد لهذه العملية
            _tempPath = Path.Combine(Path.GetTempPath(), "Uniceps_Export_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempPath);
            _config = new ExportConfig();
        }
        public void AddFile(object data, string fileName, string fileType)
        {
            // 1. تجهيز الـ UniFile
            var uniFile = new UniFile
            {
                Meta = new MetaData
                {
                    Source = "Uniceps Desktop",
                    SchemaVersion = 1,
                    CreatedAt = DateTime.UtcNow,
                    FileType = fileType
                },
                Data = data
            };

            string fullPath = Path.Combine(_tempPath, UniFileHelper.EnsureUniExtension(fileName));
            FileManager.Write(uniFile, fullPath);
            _config.ExportedFiles.Add(new FileEntry
            {
                FileName = Path.GetFileName(fullPath),
                DataType = fileType,
                Timestamp = DateTime.UtcNow
            });
        }

        public void Complete(string finalZipPath)
        {
            // 1. كتابة ملف الـ Config النهائي في المجلد المؤقت
            string configPath = Path.Combine(_tempPath, "config.unx");
            JsonHelper.Write(configPath, _config);

            // 2. ضغط المجلد بالكامل إلى ملف .unxpro (أو اللاحقة التي تفضلها)
            if (File.Exists(finalZipPath)) File.Delete(finalZipPath);

            System.IO.Compression.ZipFile.CreateFromDirectory(_tempPath, finalZipPath);

            // 3. تنظيف المجلد المؤقت
            Directory.Delete(_tempPath, true);
        }
    }
}
