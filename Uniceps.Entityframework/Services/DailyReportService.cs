using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class DailyReportService : IDailyReportService
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public DailyReportService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Credit>> GetCredits(DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().Include(x => x.EmpPerson).AsNoTracking().Where(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day).ToListAsync();
                return entities;
            }
        }

        public async Task<IEnumerable<Expenses>> GetExpenses(DateTime date)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Expenses>? entities = await context.Set<Expenses>().Where(x => x.date.Year == date.Year && x.date.Month == date.Month && x.date.Day == date.Day).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<PlayerPayment>> GetPayments(DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Where(x => x.PayDate.Month == date.Month && x.PayDate.Year == date.Year && x.PayDate.Day == date.Day)
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptions(DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.RollDate.Day == date.Day && x.RollDate.Month == date.Month && x.RollDate.Year == date.Year)
                    .ToListAsync();
                return entities;
            }
        }
    }
}
