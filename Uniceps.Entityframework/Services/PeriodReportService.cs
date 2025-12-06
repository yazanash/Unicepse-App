using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class PeriodReportService : IPeriodReportService
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PeriodReportService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Expenses>> GetExpenses(DateTime from, DateTime to)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Expenses>? entities = await context.Set<Expenses>().Where(x => x.date >= from && x.date <= to).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<PlayerPayment>> GetPayments(DateTime from, DateTime to)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                   .Where(x => x.PayDate >= from && x.PayDate <= to)
                    .ToListAsync();
                return entities;
            }
        }
    }
}
