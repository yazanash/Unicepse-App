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
    public interface IExportDataProvider
    {
        Task<IEnumerable<Player>> GetFullPlayersDataAsync();
        Task<IEnumerable<Sport>> GetSportsWithTrainersAsync();
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<IEnumerable<Expenses>> GetAllExpensesAsync();
        Task<IEnumerable<Metric>> GetAllMetricsAsync();
        Task<IEnumerable<Credit>> GetAllCreditsAsync();
        Task<IEnumerable<DailyPlayerReport>> GetAllAttendencesAsync();
        Task<IEnumerable<RoutineModel>> GetAllRoutines();
    }
}
