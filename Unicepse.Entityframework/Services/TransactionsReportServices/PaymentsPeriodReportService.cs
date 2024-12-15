using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.TransactionsReportServices
{
    public class PaymentsPeriodReportService : IPeriodReportService<PlayerPayment>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public PaymentsPeriodReportService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<PlayerPayment>> GetAll(DateTime dateFrom, DateTime dateTo)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).Include(x => x.Subscription!.Sport).Include(x => x.Subscription!.Trainer).AsNoTracking().Where(x => x.PayDate >= dateFrom && x.PayDate <= dateTo && x.DataStatus != DataStatus.ToDelete)
                    .ToListAsync();
                return entities;
            }
        }
    }
}
