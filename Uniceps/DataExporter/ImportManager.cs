using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Models.Sport;
using Uniceps.DataExporter.FileContainers;
using Uniceps.DataExporter.Mappers;
using Uniceps.Entityframework.DataExportProvider;
using Uniceps.FileSystem;
using Uniceps.FileSystem.Helpers;
using Uniceps.FileSystem.Models;

namespace Uniceps.DataExporter
{
    public class ImportManager
    {
        private readonly IImportDataProvider _importDataProvider;

        public ImportManager(IImportDataProvider importDataProvider)
        {
            _importDataProvider = importDataProvider;
        }
        public async Task<bool> RunImportProcess(string zipFilePath, IProgress<int>? progress = null)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "Uniceps_Import_" + Guid.NewGuid().ToString("N"));

            try
            {
                // 1. فك الضغط للمجلد المؤقت
                ZipFile.ExtractToDirectory(zipFilePath, tempPath);
                progress?.Report(10);

                // 2. قراءة ملف الكونفيغ والتأكد من وجود الملفات
                string configFilePath = Path.Combine(tempPath, "config.unx");
                var config = JsonHelper.Read<ExportConfig>(configFilePath);
                if (config == null) throw new Exception("ملف التوصيف config.unx مفقود!");

                foreach (var fileEntry in config.ExportedFiles)
                {
                    string expectedFilePath = Path.Combine(tempPath, fileEntry.FileName);

                    // التأكد من وجود الملف في المجلد
                    if (!File.Exists(expectedFilePath))
                    {
                        throw new Exception($"الملف المذكور في التوصيف مفقود: {fileEntry.FileName}");
                    }

                }
                var actualFiles = Directory.GetFiles(tempPath, "*.unx");
                if (actualFiles.Length != config.ExportedFiles.Count + 1) // +1 لملف الـ config نفسه
                {
                    throw new Exception("تنبيه: تم العثور على ملفات إضافية غير معروفة داخل الحزمة!");
                }
                ReferancesFileContainer refFileContainer=new();
                PlayerFileContainer playerFileContainer = new();
                PlayerReferancesFileContainer playerReferancesFileContainer = new();
                AccountantFileContainer accountantFileContainer = new();
                RoutinesFileContainer routinesFileContainer= new();

                if (config.ExportedFiles.Any(x => x.FileName == "references.unx"))
                {
                    var refFile = FileManager.Read(Path.Combine(tempPath, "references.unx"));
                    if (refFile != null && refFile.Data != null)
                        refFileContainer = JsonConvert.DeserializeObject<ReferancesFileContainer>(refFile.Data.ToString()!) ?? new();

                }
                if (config.ExportedFiles.Any(x => x.FileName == "players.unx"))
                {
                    var playerFile = FileManager.Read(Path.Combine(tempPath, "players.unx"));
                    if (playerFile != null && playerFile.Data != null)
                        playerFileContainer = JsonConvert.DeserializeObject<PlayerFileContainer>(playerFile.Data.ToString()!) ?? new();

                }
                if (config.ExportedFiles.Any(x => x.FileName == "routines.unx"))
                {
                    var routineFile = FileManager.Read(Path.Combine(tempPath, "routines.unx"));
                    if (routineFile != null && routineFile.Data != null)
                        routinesFileContainer = JsonConvert.DeserializeObject<RoutinesFileContainer>(routineFile.Data.ToString()!) ?? new();


                }
                if (config.ExportedFiles.Any(x => x.FileName == "player_referances.unx"))
                {
                    var detailsFile = FileManager.Read(Path.Combine(tempPath, "player_referances.unx"));
                    if (detailsFile != null && detailsFile.Data != null)
                        playerReferancesFileContainer = JsonConvert.DeserializeObject<PlayerReferancesFileContainer>(detailsFile.Data.ToString()!) ?? new();

                }
                if (config.ExportedFiles.Any(x => x.FileName == "accounting.unx"))
                {
                    var accFile = FileManager.Read(Path.Combine(tempPath, "accounting.unx"));
                    if (accFile != null && accFile.Data != null)
                        accountantFileContainer = JsonConvert.DeserializeObject<AccountantFileContainer>(accFile.Data.ToString()!) ?? new();

                }

                progress?.Report(40);

                await SyncReferences(refFileContainer);
                progress?.Report(60);

                await SyncPlayers(playerFileContainer);
                progress?.Report(80);

                await SyncPlayerDetails(playerReferancesFileContainer);
                await SyncAccounting(accountantFileContainer);
                progress?.Report(90);

                await SyncRoutines(routinesFileContainer);
                progress?.Report(100);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل الاستيراد: {ex.Message}");
                return false;
            }
            finally
            {
                // تنظيف المجلد المؤقت دائماً
                if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            }
        }
        public async Task SyncReferences(ReferancesFileContainer referancesFileContainer)
        {
            List<Sport> sports = referancesFileContainer.SportDtos.Select(x => SportMapper.FromDto(x)).ToList();
            List<Employee> employees = referancesFileContainer.EmployeeDtos.Select(x => EmployeeMapper.FromDto(x)).ToList();
            await _importDataProvider.SyncReferences(sports, employees);
        }
        public async Task SyncRoutines(RoutinesFileContainer routinesFileContainer)
        {
          
            List<RoutineModel> routines = routinesFileContainer.RoutineDtos.Select(x => RoutineFileMapper.FromDto(x)).ToList();
            await _importDataProvider.SyncRoutines(routines);
        }
        public async Task SyncPlayers(PlayerFileContainer playerFileContainer)
        {
            List<Player> players = playerFileContainer.PlayerDtos.Select(x => PlayerMapper.FromDto(x)).ToList();
            await _importDataProvider.SyncPlayers(players);
        }
        public async Task SyncPlayerDetails(PlayerReferancesFileContainer playerReferancesFileContainer)
        {
            List<Metric> metrics = playerReferancesFileContainer.MetricDtos.Select(x => MetricMapper.FromDto(x)).ToList();
            List<DailyPlayerReport> dailyPlayerReports = playerReferancesFileContainer.AttendancesDtos.Select(x => AttendanceMapper.FromDto(x)).ToList();
            await _importDataProvider.SyncPlayerDetails(metrics, dailyPlayerReports);
        }
        public async Task SyncAccounting(AccountantFileContainer accountantFileContainer)
        {
            List<Credit> credits = accountantFileContainer.CreditDtos.Select(x => CreditMapper.FromDto(x)).ToList();
            List<Expenses> expenses = accountantFileContainer.ExpenseDtos.Select(x => ExpensesMapper.FromDto(x)).ToList();
            await _importDataProvider.SyncAccounting(credits, expenses);
        }

    }
}
