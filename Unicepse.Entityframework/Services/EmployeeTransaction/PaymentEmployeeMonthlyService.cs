using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.EmployeeTransaction
{
    public class PaymentEmployeeMonthlyService : IEmployeeMonthlyTransaction<PlayerPayment>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public PaymentEmployeeMonthlyService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<PlayerPayment>> GetAll(Employee trainer, DateTime date)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).AsNoTracking()
                    .Include(x => x.Subscription!.Player).AsNoTracking()
                    .Include(x => x.Subscription!.Sport).AsNoTracking()
                    .Include(x => x.Subscription!.Trainer).AsNoTracking()
                    .Where(x => x.Subscription!.Trainer!.Id == trainer.Id &&
                    (
                   (x.PayDate.Month == date.Month && x.PayDate.Year == date.Year ||
                    x.To.Month == date.Month && x.To.Year == date.Year) && x.DataStatus != DataStatus.ToDelete
                    )
                    ).AsNoTracking()
                    .ToListAsync();
                return entities;
            }
        }
    }
}
