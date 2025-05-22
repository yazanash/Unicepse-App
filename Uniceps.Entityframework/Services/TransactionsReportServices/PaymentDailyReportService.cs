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
    public class PaymentDailyReportService : IDailyTransactionService<PlayerPayment>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public PaymentDailyReportService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<PlayerPayment>> GetAll(DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).Include(x => x.Subscription!.Sport).Include(x => x.Subscription!.Trainer).AsNoTracking()
                    .Where(x => x.PayDate.Month == date.Month && x.PayDate.Year == date.Year && x.PayDate.Day == date.Day && x.DataStatus != DataStatus.ToDelete)
                    .ToListAsync();
                return entities;
            }
        }
    }
}
