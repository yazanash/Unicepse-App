using Microsoft.EntityFrameworkCore;
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
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.DataExportProvider
{
    public class ExportDataProviderService : IExportDataProvider
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public ExportDataProviderService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Credit>> GetAllCreditsAsync()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<Expenses>> GetAllExpensesAsync()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<Expenses>? entities = await context.Set<Expenses>().AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<Metric>> GetAllMetricsAsync()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<Metric>? entities = await context.Set<Metric>().AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<DailyPlayerReport>> GetAllAttendencesAsync()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<DailyPlayerReport>? entities = await context.Set<DailyPlayerReport>().AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<Employee>? entities = await context.Set<Employee>().Include(x => x.Sports).AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<Player>> GetFullPlayersDataAsync()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<Player>? entities = await context.Set<Player>().Include(x=>x.Subscriptions).ThenInclude(x=>x.Payments).AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<Sport>> GetSportsWithTrainersAsync()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<Sport>? entities = await context.Set<Sport>().Include(x=>x.Trainers).AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<RoutineModel>> GetAllRoutines()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<RoutineModel> entities = await context.Set<RoutineModel>().Include(x => x.Days).ThenInclude(x => x.RoutineItems).ThenInclude(x => x.Exercise)
                .Include(x => x.Days).ThenInclude(x => x.RoutineItems).ThenInclude(x => x.Sets).AsNoTracking().ToListAsync();
            return entities;
        }
    }
}
