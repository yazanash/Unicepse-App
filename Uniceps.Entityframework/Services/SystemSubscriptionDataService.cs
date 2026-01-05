using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class SystemSubscriptionDataService : ISystemSubscriptionDataService
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public SystemSubscriptionDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task ClearOldSubscription()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                await context.Set<SystemSubscription>().ExecuteDeleteAsync();
                await context.SaveChangesAsync();
            }
        }

        public async Task<SystemSubscription> Create(SystemSubscription entity)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<SystemSubscription> CreatedResult = await context.Set<SystemSubscription>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<SystemSubscription?> GetActiveSubscription()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            SystemSubscription? entity = await context.Set<SystemSubscription>().AsNoTracking().FirstOrDefaultAsync(x =>
            x.EndDate >= DateTime.Now && x.StartDate <= DateTime.Now);
            return entity;
        }

        public async Task<SystemSubscription> Update(SystemSubscription entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            context.Set<SystemSubscription>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
