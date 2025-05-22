using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.TransactionsReportServices
{
    public class PaymentsPeriodReportService : IPeriodReportService<PlayerPayment>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PaymentsPeriodReportService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<PlayerPayment>> GetAll(DateTime dateFrom, DateTime dateTo)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).Include(x => x.Subscription!.Sport).Include(x => x.Subscription!.Trainer).AsNoTracking().Where(x => x.PayDate >= dateFrom && x.PayDate <= dateTo && x.DataStatus != DataStatus.ToDelete)
                    .ToListAsync();
                return entities;
            }
        }
    }
}
