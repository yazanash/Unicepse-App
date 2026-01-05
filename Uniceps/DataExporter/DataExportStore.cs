using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;

namespace Uniceps.DataExporter
{
    public class DataExportStore
    {
        private readonly DataExportManager _exportManager;
        private readonly ImportManager _importManager;
        public DataExportStore(DataExportManager exportManager, ImportManager importManager)
        {
            _exportManager = exportManager;
            _importManager = importManager;
        }

        public event Action<int>? ProgressChanged;
        public event Action<string>? StatusChanged;
        public event Action<bool, string>? ExportFinished;
        public event Action<bool, string>? ImportFinished;
        public async Task StartExportAsync()
        {
            try
            {
                string folderPath = Properties.Settings.Default.BackupPath;

                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    ExportFinished?.Invoke(false, "مسار التصدير غير محدد. يرجى ضبطه من إعدادات النظام.");
                    return;
                }

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = $"Uniceps_Backup_{DateTime.Now:yyyyMMdd_HHmm}.unxpro";
                string fullPath = Path.Combine(folderPath, fileName);

                // 4. إعداد الـ Progress
                var progress = new Progress<int>(p =>
                {
                    ProgressChanged?.Invoke(p);
                    UpdateStatusText(p);
                });

                StatusChanged?.Invoke("بدء تجهيز البيانات...");

                // 5. استدعاء المانجر لتنفيذ العملية
                bool success = await _exportManager.RunExportProcess(fullPath, progress);

                if (success)
                {
                    Properties.Settings.Default.LastBackup = DateTime.Now;
                    Properties.Settings.Default.Save(); 

                    ExportFinished?.Invoke(true, "تمت عملية النسخ الاحتياطي بنجاح.");
                }
                else
                {
                    ExportFinished?.Invoke(false, "فشلت العملية. يرجى التأكد من عدم وجود ملفات مفتوحة أو قيود وصول.");
                }
            }
            catch (Exception ex)
            {
                ExportFinished?.Invoke(false, $"خطأ غير متوقع: {ex.Message}");
            }
        }
        private void UpdateStatusText(int progress)
        {
            if (progress <= 25) StatusChanged?.Invoke("جاري جلب بيانات الموظفين والرياضات...");
            else if (progress <= 50) StatusChanged?.Invoke("جاري معالجة بيانات اللاعبين والاشتراكات...");
            else if (progress <= 75) StatusChanged?.Invoke("جاري استخراج السجلات المالية والتقارير...");
            else if (progress < 100) StatusChanged?.Invoke("جاري ضغط الملف النهائي وحفظه...");
            else StatusChanged?.Invoke("اكتملت العملية.");
        }
        public async Task StartImportAsync(string filePath)
        {
            try
            {
                StatusChanged?.Invoke("جاري فك ضغط الحزمة والتحقق من الملفات...");
                ProgressChanged?.Invoke(0);

                var progress = new Progress<int>(p =>
                {
                    ProgressChanged?.Invoke(p);
                    UpdateStatusTextForImport(p);
                });


                bool result = await _importManager.RunImportProcess(filePath, progress);

                if (result)
                    ImportFinished?.Invoke(true, "تم استرجاع البيانات بنجاح!");
                else
                    ImportFinished?.Invoke(false, "فشلت عملية الاسترجاع، تأكد من سلامة الملف.");
            }
            catch (Exception ex)
            {
                ImportFinished?.Invoke(false, $"خطأ غير متوقع: {ex.Message}");
            }
        }
        private void UpdateStatusTextForImport(int progress)
        {
            if (progress <= 25) StatusChanged?.Invoke("جاري استعادة بيانات الموظفين والرياضات...");
            else if (progress <= 50) StatusChanged?.Invoke("جاري معالجة بيانات اللاعبين والاشتراكات...");
            else if (progress <= 75) StatusChanged?.Invoke("جاري اضافة السجلات المالية والتقارير...");
            else if (progress < 100) StatusChanged?.Invoke("انهاء عمليات الحفظ");
            else StatusChanged?.Invoke("اكتملت العملية.");
        }
      
    }
}
