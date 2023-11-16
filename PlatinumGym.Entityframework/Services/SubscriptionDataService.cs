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
            e.Sport.Id==subscription.Sport.Id && e.EndDate >= subscription.RollDate);
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

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Subscription> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Subscription>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Subscription> Update(Subscription entity)
        {
            throw new NotImplementedException();
        }
    }
}
