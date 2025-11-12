using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Subscription;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class SubscriptionDataService : IDataService<Subscription>
    {

        private readonly UnicepsDbContextFactory _contextFactory;

        public SubscriptionDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Subscription> CheckIfSubscriptionExist(Subscription subscription)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Subscription? entity = await context.Set<Subscription>().AsNoTracking().FirstOrDefaultAsync((e) => e.Player!.Id == subscription.Player!.Id &&
            e.Sport!.Id == subscription.Sport!.Id && e.EndDate >= subscription.RollDate);
            return entity!;
        }
        public async Task<Subscription> Create(Subscription entity)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                Subscription existed_subscription = await CheckIfSubscriptionExist(entity);
                if (existed_subscription != null)
                    throw new ConflictException("هذا اللاعب مسجل في هذه الرياضة مسبقا ");
                context.Entry(entity.Sport!).State = EntityState.Detached;
                if (context.Entry(entity.Sport!).State == EntityState.Detached)
                    context.Entry(entity.Sport!).State = EntityState.Unchanged;

                context.Entry(entity.Player!).State = EntityState.Detached;
                if (context.Entry(entity.Player!).State == EntityState.Detached)
                    context.Entry(entity.Player!).State = EntityState.Unchanged;

                if (entity.Trainer != null)
                {
                    context.Entry(entity.Trainer).State = EntityState.Detached;
                    if (context.Entry(entity.Trainer).State == EntityState.Detached)
                        context.Entry(entity.Trainer).State = EntityState.Unchanged;
                }

                EntityEntry<Subscription> CreatedResult = await context.Set<Subscription>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Subscription? entity = await context.Set<Subscription>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<Subscription>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Subscription> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Subscription? entity = await context.Set<Subscription>().Include(e => e.Sport).AsNoTracking()
                .Include(e => e.Player).AsNoTracking()
                .Include(x => x.Payments).AsNoTracking()
                .Include(e => e.Trainer).AsNoTracking()
                .FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }
        public async Task<IEnumerable<Subscription>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().Include(x => x.Trainer).AsNoTracking()
                    .Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Payments).AsNoTracking()
                    .Include(x => x.Sport).AsNoTracking().Select(p => new Subscription
                    {
                        Sport = p.Sport,
                        LastCheck = p.LastCheck,
                        TrainerId = p.TrainerId,
                        Trainer = p.Trainer,
                        PrevTrainer_Id = p.PrevTrainer_Id,
                        Player = p.Player,
                        RollDate = p.RollDate,
                        Price = p.Price,
                        OfferValue = p.OfferValue,
                        OfferDes = p.OfferDes,
                        PriceAfterOffer = p.PriceAfterOffer,
                        MonthCount = p.MonthCount,
                        DaysCount = p.DaysCount,
                        IsStopped = p.IsStopped,
                        IsPaid = p.Payments!.Any() && p.Payments!.Sum(x => x.PaymentValue) >= p.PriceAfterOffer,
                        PaidValue = p.Payments!.Sum(x => x.PaymentValue),
                        RestValue = p.Payments!.Sum(x => x.PaymentValue) - p.PriceAfterOffer,
                        EndDate = p.EndDate,
                        LastPaid = p.LastPaid,

                    }).ToListAsync();
                return entities;
            }
        }
        public async Task<Subscription> Update(Subscription entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            Subscription existed_subscription = await Get(entity.Id);
            if (existed_subscription == null)
                throw new NotExistException("هذا السجل غير موجود");
            //Subscription ex_subscription = await CheckIfSubscriptionExist(entity);
            //if (ex_subscription != null&&existed_subscription.Id!=entity.Id)
            //    throw new ConflictException("هذا اللاعب مسجل في هذه الرياضة مسبقا ");
            context.Entry(entity).State = EntityState.Detached;
            context.Entry(entity.Player!).State = EntityState.Detached;
            context.Entry(entity.Sport!).State = EntityState.Detached;

            if (context.Entry(entity.Player!).State == EntityState.Detached)
                context.Entry(entity.Player!).State = EntityState.Unchanged;

            if (context.Entry(entity).State == EntityState.Detached)
                context.Entry(entity).State = EntityState.Modified;

            if (context.Entry(entity.Sport!).State == EntityState.Detached)
                context.Entry(entity.Sport!).State = EntityState.Unchanged;


            if (entity.Trainer != null)
            {
                if (entity.Trainer.Id == 0)
                    entity.Trainer = null;
                else
                {
                    context.Entry(entity.Trainer!).State = EntityState.Detached;
                    if (context.Entry(entity.Trainer).State == EntityState.Detached)
                        context.Entry(entity.Trainer).State = EntityState.Unchanged;
                }

            }

            context.Set<Subscription>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

    }
}
