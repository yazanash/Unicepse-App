using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Entityframework.Services
{
    public class PaymentDataService : IDataService<PlayerPayment>, IGetPlayerTransactionService<PlayerPayment>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PaymentDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PlayerPayment> Create(PlayerPayment entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            var subscription = await context.Set<Subscription>()
       .Include(s => s.Payments)
       .FirstOrDefaultAsync(s => s.Id == entity.SubscriptionId);

            if (subscription == null)
                throw new NotExistException("الاشتراك غير موجود");

            // نحسب بداية التغطية
            var lastPayment = subscription.Payments?
                .OrderByDescending(p => p.CoveredTo)
                .FirstOrDefault();

            if (lastPayment != null)
                entity.CoveredFrom = lastPayment.CoveredTo.AddDays(1);
            else
                entity.CoveredFrom = subscription.RollDate;

            // نحسب نهاية التغطية حسب قيمة الدفع
            int totalDays = (subscription.EndDate - subscription.RollDate).Days + 1;
            double dailyPrice = subscription.PriceAfterOffer / totalDays;
            int coveredDays = (int)Math.Round(entity.PaymentValue / dailyPrice);
            entity.CoveredTo = entity.CoveredFrom.AddDays(coveredDays - 1);
            EntityEntry<PlayerPayment> CreatedResult = await context.Set<PlayerPayment>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment? entity = await context.Set<PlayerPayment>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<PlayerPayment>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PlayerPayment> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment? entity = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
               .FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<PlayerPayment>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<PlayerPayment>> GetAll(Player player)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Where(x => x.Player!.Id == player.Id).AsNoTracking()
                   .AsNoTracking().ToListAsync();
                return entities;
            }
        }
        public async Task<PlayerPayment> Update(PlayerPayment entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment existed_payment = await Get(entity.Id);
            if (existed_payment == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Entry(entity).State = EntityState.Detached;
            if (context.Entry(entity).State == EntityState.Detached)
            {
                // Attach the entity
                context.Entry(entity).State = EntityState.Modified;
            }
            var subscription = await context.Set<Subscription>()
      .Include(s => s.Payments)
      .FirstOrDefaultAsync(s => s.Id == entity.SubscriptionId);

            if (subscription == null)
                throw new NotExistException("الاشتراك غير موجود");

            var payments = subscription.Payments!
                .OrderBy(p => p.CoveredFrom)
                .ToList();

            // استبدال الدفعة المعدلة بالقيمة الجديدة
            var paymentIndex = payments.FindIndex(p => p.Id == entity.Id);
            if (paymentIndex < 0)
                throw new NotExistException("الدفعة غير موجودة");

            payments[paymentIndex] = entity;

            // إعادة حساب CoveredFrom و CoveredTo لكل الدفعات
            for (int i = 0; i < payments.Count; i++)
            {
                var prevEnd = i == 0 ? subscription.RollDate.AddDays(-1) : payments[i - 1].CoveredTo;
                payments[i].CoveredFrom = prevEnd.AddDays(1);

                int totalDays = (subscription.EndDate - subscription.RollDate).Days + 1;
                double dailyPrice = subscription.PriceAfterOffer / totalDays;
                int coveredDays = (int)Math.Round(payments[i].PaymentValue / dailyPrice);

                payments[i].CoveredTo = payments[i].CoveredFrom.AddDays(coveredDays - 1);


                context.Entry(payments[i]).State = EntityState.Modified;
            }

            //context.Attach(entity);
            context.Set<PlayerPayment>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
