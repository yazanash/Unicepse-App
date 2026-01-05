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
using Uniceps.DataExporter.FileContainers;
using Uniceps.DataExporter.Mappers;

namespace Uniceps.DataExporter.ExportServices
{
    public static class FilesExportDataService
    {
        public static AccountantFileContainer LoadAccountantData(List<Expenses> expenses, List<Credit> credits)
        {
            return new AccountantFileContainer()
            {
                CreditDtos = credits.Select(c => CreditMapper.ToDto(c)).ToList(),
                ExpenseDtos = expenses.Select(e => ExpensesMapper.ToDto(e)).ToList(),
            };
        }
        public static PlayerFileContainer LoadPlayerData(List<Player> players)
        {
            return new PlayerFileContainer()
            {
                PlayerDtos = players.Select(p => PlayerMapper.ToDto(p)).ToList(),
            };
        }
        public static PlayerReferancesFileContainer LoadPlayerReferancesData(List<DailyPlayerReport> dailyPlayerReports, List<Metric> metrics)
        {
            return new PlayerReferancesFileContainer()
            {
                AttendancesDtos = dailyPlayerReports.Select(a => AttendanceMapper.ToDto(a)).ToList(),
                MetricDtos = metrics.Select(m => MetricMapper.ToDto(m)).ToList(),
            };
        }
        public static ReferancesFileContainer LoadReferancesData(List<Sport> sports, List<Employee> employees)
        {
            return new ReferancesFileContainer()
            {
                SportDtos = sports.Select(s => SportMapper.ToDto(s)).ToList(),
                EmployeeDtos = employees.Select(e => EmployeeMapper.ToDto(e)).ToList(),
            };
        }
        public static RoutinesFileContainer LoadRoutinesData(List<RoutineModel> routines)
        {
            return new RoutinesFileContainer()
            {
                RoutineDtos = routines.Select(s => RoutineFileMapper.ToDto(s)).ToList(),
            };
        }
    }
}
