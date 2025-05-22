using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.SportTransactions
{
    public class SubscriptionSportMonthlyReportService : ISportMonthlyTransactions<Subscription>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public SubscriptionSportMonthlyReportService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Subscription>> GetAll(Sport sport, DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.Sport!.Id == sport.Id
                && (x.RollDate.Month == date.Month && x.RollDate.Year == date.Year
                || x.EndDate.Month == date.Month && x.EndDate.Year == date.Year)).Include(x => x.Trainer).AsNoTracking()
                    .Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Sport).ToListAsync();
                return entities;
            }
        }
    }
}
