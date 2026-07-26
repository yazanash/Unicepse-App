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

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var subscription = await context.Set<Subscription>()
                    .FirstOrDefaultAsync(s => s.Id == entity.SubscriptionId);

                if (subscription == null)
                    throw new NotExistException("الاشتراك غير موجود");

                if (subscription.TrainerId.HasValue)
                {
                    var trainer = await context.Set<Employee>().FindAsync(subscription.TrainerId);
                    if (trainer == null) throw new Exception("هذا المدرب غير موجود");

                    if (entity.PayDate < (trainer.LastClosingDate ?? trainer.StartDate))
                    {
                        throw new Exception("التاريخ المدخل يقع ضمن فترة مالية مغلقة ومؤرشفة.");
                    }
                }

                var lastPayment = await context.Set<PlayerPayment>()
                    .Where(p => p.SubscriptionId == entity.SubscriptionId)
                    .OrderByDescending(p => p.CoveredTo)
                    .FirstOrDefaultAsync();

                if (lastPayment != null)
                    entity.CoveredFrom = lastPayment.CoveredTo.AddDays(1);
                else
                    entity.CoveredFrom = subscription.RollDate;

                int totalDays = (subscription.EndDate - subscription.RollDate).Days + 1;
                double dailyPrice = subscription.PriceAfterOffer / totalDays;
                int coveredDays = (int)Math.Round(entity.PaymentValue / dailyPrice);
                entity.CoveredTo = entity.CoveredFrom.AddDays(coveredDays - 1);
                entity.SubscriptionSyncId = subscription.SyncId;
                entity.PlayerSyncId = subscription.PlayerSyncId;
                EntityEntry<PlayerPayment> createdResult = await context.Set<PlayerPayment>().AddAsync(entity);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();

                return createdResult.Entity;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
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
        public async Task<IEnumerable<PlayerPayment>> GetAllByPlayer(int playerId)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Where(x => x.Player!.Id == playerId).AsNoTracking()
                   .AsNoTracking().ToListAsync();
                return entities;
            }
        }
        public async Task<PlayerPayment> Update(PlayerPayment entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var subscription = await context.Set<Subscription>()
                    .FirstOrDefaultAsync(s => s.Id == entity.SubscriptionId);

                if (subscription == null)
                    throw new NotExistException("الاشتراك غير موجود");

                if (subscription.TrainerId.HasValue)
                {
                    var trainer = await context.Set<Employee>().FindAsync(subscription.TrainerId);
                    if (trainer == null) throw new Exception("هذا المدرب غير موجود");
                    if (entity.PayDate < (trainer.LastClosingDate ?? trainer.StartDate))
                    {
                        throw new Exception("التاريخ المدخل يقع ضمن فترة مالية مغلقة ومؤرشفة.");
                    }
                }
              
                var existedPayment = await context.Set<PlayerPayment>().FirstOrDefaultAsync(p => p.Id == entity.Id);
                if (existedPayment == null)
                    throw new NotExistException("هذا السجل غير موجود في هذا الاشتراك");

                double oldAmount = existedPayment.PaymentValue;
                double newAmount = entity.PaymentValue;

                existedPayment.PaymentValue = entity.PaymentValue;
                existedPayment.PayDate = entity.PayDate;
                existedPayment.Des = entity.Des;

                var payments = await context.Set<PlayerPayment>().Where(x=>x.SubscriptionId == existedPayment.SubscriptionId)
                    .OrderBy(p => p.CoveredFrom)
                    .ToListAsync();

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

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return existedPayment;
            }
            catch (Exception )
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
