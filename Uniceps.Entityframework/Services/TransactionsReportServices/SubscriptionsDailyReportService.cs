using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.TransactionsReportServices
{
    public class SubscriptionsDailyReportService : IDailyTransactionService<Subscription>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public SubscriptionsDailyReportService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Subscription>> GetAll(DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.RollDate.Day == date.Day && x.RollDate.Month == date.Month && x.RollDate.Year == date.Year)
                    .Include(x => x.Trainer).AsNoTracking()
                    .Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Sport).ToListAsync();
                return entities;
            }
        }
    }
}
