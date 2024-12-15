using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.EmployeeTransaction
{
    public class SubscriptionEmployeeMonthlyService : IEmployeeMonthlyTransaction<Subscription>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public SubscriptionEmployeeMonthlyService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Subscription>> GetAll(Employee trainer, DateTime date)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.Trainer!.Id == trainer.Id
                && ((x.RollDate.Month == date.Month && x.RollDate.Year == date.Year)
                || (x.EndDate.Month == date.Month && x.EndDate.Year == date.Year))).Include(x => x.Trainer).AsNoTracking()
                    .Include(x => x.Player).AsNoTracking().Include(x => x.Sport!.Trainers).AsNoTracking()
                    .Include(x => x.Sport).ToListAsync();
                return entities;
            }
        }
    }
}
