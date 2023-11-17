using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.Sport;
using PlatinumGym.Core.Models.Subscription;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.Services
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
            Subscription? entity = await context.Set<Subscription>().FirstOrDefaultAsync((e) => e.Player.Id == subscription.Player.Id &&
            e.Sport.Id == subscription.Sport.Id && e.EndDate >= subscription.RollDate);
            return entity;
        }
        public async Task<Subscription> Create(Subscription entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                Subscription existed_subscription = await CheckIfSubscriptionExist(entity);
                if (existed_subscription != null)
                    throw new ConflictException();
                context.Attach<Sport>(entity.Sport);
                context.Attach<Player>(entity.Player);
                context.Attach<Employee>(entity.Trainer);
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
            Subscription? entity = await context.Set<Subscription>().Include(e => e.Sport).Include(e => e.Player).Include(e => e.Trainer).FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }


        public async Task<IEnumerable<Subscription>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().Include(x => x.Trainer)
                    .Include(x => x.Player)
                    .Include(x => x.Sport).ToListAsync();
                return entities;
            }
        }

        public async Task<Subscription> Update(Subscription entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Subscription existed_subscription = await Get(entity.Id);
            if (existed_subscription == null)
                throw new NotExistException();
            context.Attach<Sport>(entity.Sport);
            context.Attach<Player>(entity.Player);
            context.Attach<Employee>(entity.Trainer);
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
            if(existed_subscription.IsMoved)
                throw new MovedBeforeException();
            context.Attach(entity.Sport!);
            context.Attach(entity.Player!);
            context.Attach(trainer);
            entity.PrevTrainer_Id = entity.Trainer!.Id;
            entity.Trainer = trainer;
            entity.IsMoved = true;
            entity.LastCheck = movedate;
            context.Set<Subscription>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<Subscription> Stop(Subscription entity,DateTime stop_date)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Subscription existed_subscription = await Get(entity.Id);
            if (existed_subscription == null)
                throw new NotExistException();
            entity.EndDate = stop_date;
            entity.IsStopped = true;
            int days = Convert.ToInt32((stop_date - entity.RollDate).TotalDays);
            double dayPrice = entity.PriceAfterOffer / entity.Sport!.DaysCount;
            entity.PriceAfterOffer = dayPrice * days;
            context.Set<Subscription>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
