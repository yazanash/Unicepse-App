using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Entityframework.Services
{
    public class SubscriptionDataService : IDataService<Subscription>
    {

        private readonly PlatinumGymDbContextFactory _contextFactory;

        public SubscriptionDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Subscription> CheckIfSubscriptionExist(Subscription subscription)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Subscription? entity = await context.Set<Subscription>().AsNoTracking().FirstOrDefaultAsync((e) => e.Player!.Id == subscription.Player!.Id &&
            e.Sport!.Id == subscription.Sport!.Id && e.EndDate >= subscription.RollDate);
            return entity!;
        }
        public async Task<Subscription> Create(Subscription entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                Subscription existed_subscription = await CheckIfSubscriptionExist(entity);
                if (existed_subscription != null)
                    throw new ConflictException();

                context.Attach<Sport>(entity.Sport!);
                context.Attach<Player>(entity.Player!);
                if (entity.Trainer != null)
                    context.Attach<Employee>(entity.Trainer!);
                EntityEntry<Subscription> CreatedResult = await context.Set<Subscription>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Subscription? entity = await context.Set<Subscription>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            context.Set<Subscription>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Subscription> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Subscription? entity = await context.Set<Subscription>().Include(e => e.Sport).AsNoTracking()
                .Include(e => e.Player).AsNoTracking()
                .Include(x => x.Payments).AsNoTracking()
                .Include(e => e.Trainer).AsNoTracking()
                .FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }


        public async Task<IEnumerable<Subscription>> GetAll(Player player)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.Player!.Id == player.Id).Include(x => x.Trainer)
                    .Include(x => x.Player).AsNoTracking().Include(x => x.Sport!.Trainers).AsNoTracking()
                    .Include(x => x.Sport).AsNoTracking().Include(x => x.Payments).AsNoTracking().ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Subscription>> GetAll(Employee trainer)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.Trainer!.Id == trainer.Id).Include(x => x.Trainer).AsNoTracking()
                    .Include(x => x.Player).AsNoTracking().Include(x => x.Sport!.Trainers).AsNoTracking()
                    .Include(x => x.Sport).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Subscription>> GetAll(Sport sport, DateTime date)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.Sport!.Id == sport.Id
                && ((x.RollDate.Month == date.Month && x.RollDate.Year == date.Year)
                || (x.EndDate.Month == date.Month && x.EndDate.Year == date.Year))).Include(x => x.Trainer).AsNoTracking()
                    .Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Sport).ToListAsync();
                return entities;
            }
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

        public async Task<IEnumerable<Subscription>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().Include(x => x.Trainer).AsNoTracking()
                    .Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Sport).AsNoTracking().ToListAsync();
                return entities;
            }
        }

        public async Task<Subscription> Update(Subscription entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            Subscription existed_subscription = await Get(entity.Id);
            if (existed_subscription == null)
                throw new NotExistException();
            context.Entry(entity).State = EntityState.Detached;
            context.Entry(entity.Player!).State = EntityState.Detached;
            context.Entry(entity.Sport!).State = EntityState.Detached;
            context.Attach(entity.Player!);

            //context.Attach(entity.Sport!);

            if (context.Entry(entity).State == EntityState.Detached)
            {
                // Attach the entity
                context.Entry(entity).State = EntityState.Modified;
            }
            if (entity.Trainer != null)
                context.Entry(entity.Trainer!).State = EntityState.Detached;

            if (context.Entry(entity.Sport!).State == EntityState.Detached)
            {
                // Attach the entity
                context.Entry(entity.Sport!).State = EntityState.Unchanged;
            }
            if (entity.Trainer != null)
                if (context.Entry(entity.Trainer).State == EntityState.Detached)
                {
                    // Attach the entity
                    context.Entry(entity.Trainer).State = EntityState.Unchanged;
                }
            context.Set<Subscription>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<Subscription> MoveToNewTrainer(Subscription entity, Employee trainer, DateTime movedate)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Subscription existed_subscription = await Get(entity.Id);
            if (existed_subscription == null)
                throw new NotExistException();
            if (existed_subscription.IsMoved)
                throw new MovedBeforeException();
            foreach (var t in entity.Sport!.Trainers!)
            {
                context.Entry(t).State = EntityState.Detached;

            }

            context.Entry(entity.Sport!).State = EntityState.Unchanged;
            context.Entry(trainer).State = EntityState.Unchanged;
            context.Attach(entity.Sport!);
            context.Attach(entity.Player!);
            context.Attach(trainer);

            if (entity.Trainer != null)
                entity.PrevTrainer_Id = entity.Trainer!.Id;
            entity.Trainer = trainer;
            entity.IsMoved = true;
            entity.LastCheck = movedate;
            context.Set<Subscription>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<Subscription> Stop(Subscription entity, DateTime stop_date)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Subscription existed_subscription = await Get(entity.Id);
            if (existed_subscription == null)
                throw new NotExistException();
            //Player? player = entity.Player;
            entity.EndDate = stop_date;
            entity.IsStopped = true;
            int days = Convert.ToInt32((stop_date - entity.RollDate).TotalDays);
            double dayPrice = entity.PriceAfterOffer / entity.Sport!.DaysCount;
            entity.PriceAfterOffer = dayPrice * days;
            context.Set<Subscription>().Update(entity);
            context.Entry(entity.Player!).State = EntityState.Detached;
            if (context.Entry(entity.Player!).State == EntityState.Detached)
                context.Entry(entity.Player!).State = EntityState.Unchanged;

            context.Entry(entity.Sport).State = EntityState.Detached;
            if (entity.Trainer != null)
                context.Entry(entity.Trainer!).State = EntityState.Detached;

            context.Entry(entity.Sport).State = EntityState.Unchanged;
            if (entity.Trainer != null)
                context.Entry(entity.Trainer!).State = EntityState.Unchanged;
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
