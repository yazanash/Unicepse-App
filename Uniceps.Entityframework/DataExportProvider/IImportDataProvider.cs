using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Models.Sport;

namespace Uniceps.Entityframework.DataExportProvider
{
    public interface IImportDataProvider
    {
        Task SyncAccounting(List<Credit> credits, List<Expenses> expenses);
        Task SyncPlayerDetails(List<Metric> metrics, List<DailyPlayerReport> dailyPlayerReports);
        Task SyncPlayers(List<Player> players);
        Task SyncReferences(List<Sport> sports,List<Employee> employees);
        Task SyncRoutines(List<RoutineModel> routines);
    }
}
