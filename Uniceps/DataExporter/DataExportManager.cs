using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.DataExporter.ExportServices;
using Uniceps.Entityframework.DataExportProvider;
using Uniceps.FileSystem.Services;

namespace Uniceps.DataExporter
{
    public class DataExportManager
    {
       private readonly IExportDataProvider _exportDataProvider;

        public DataExportManager(IExportDataProvider exportDataProvider)
        {
            _exportDataProvider = exportDataProvider;
        }
        public async Task<bool> RunExportProcess(string userSelectedPath, IProgress<int>? progress = null)
        {
            UnxExportUnitOfWork? uow = null;
            try
            {
                uow = new UnxExportUnitOfWork();

                // 1. المرجعيات
                var sports = await _exportDataProvider.GetSportsWithTrainersAsync();
                var employees = await _exportDataProvider.GetEmployeesAsync();
                var refContainer = FilesExportDataService.LoadReferancesData(sports.ToList(), employees.ToList());
                uow.AddFile(refContainer, "references", "References");
                progress?.Report(25);

                // 2. اللاعبين (الشجرة الكبرى)
                var players = await _exportDataProvider.GetFullPlayersDataAsync();
                var playerContainer = FilesExportDataService.LoadPlayerData(players.ToList());
                uow.AddFile(playerContainer, "players", "PlayersFull");
                progress?.Report(50);

                var routines = await _exportDataProvider.GetAllRoutines();
                var routinesContainer = FilesExportDataService.LoadRoutinesData(routines.ToList());
                uow.AddFile(routinesContainer, "routines", "Routines");
                progress?.Report(70);

                // 3. التقارير والقياسات (الملفات الملحقة باللاعب)
                var attendances = await _exportDataProvider.GetAllAttendencesAsync();
                var metrics = await _exportDataProvider.GetAllMetricsAsync();
                var playerRefs = FilesExportDataService.LoadPlayerReferancesData(attendances.ToList(), metrics.ToList());
                uow.AddFile(playerRefs, "player_referances", "PlayerReferances");
                progress?.Report(75);

                // 4. المحاسبة
                var expenses = await _exportDataProvider.GetAllExpensesAsync();
                var credits = await _exportDataProvider.GetAllCreditsAsync();
                var accContainer = FilesExportDataService.LoadAccountantData(expenses.ToList(), credits.ToList());
                uow.AddFile(accContainer, "accounting", "Accounting");

                // 5. الختام
                uow.Complete(userSelectedPath);
                progress?.Report(100);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
