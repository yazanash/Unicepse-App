using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Server.Services
{
    public class SubscriptionPushPullDataService : IPushPullDataService<Subscription>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public SubscriptionPushPullDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Subscription>> PullDataAsync(long lastServerVersion)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Subscription>? entities = await context.Set<Subscription>().Where(x => x.ServerVersion > lastServerVersion).ToListAsync();
            return entities;
        }

        public async Task PushDataAsync(List<Subscription> data)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            long currentMaxVersion = await context.Set<Subscription>().MaxAsync(p => (long?)p.ServerVersion) ?? 0;
            foreach (Subscription subscription in data)
            {
                currentMaxVersion++;
                subscription.ServerVersion = currentMaxVersion;
                Player? player = await context.Set<Player>().FirstOrDefaultAsync(x => x.SyncId == subscription.SyncId);
                if (player == null) continue;
                Sport? sport = await context.Set<Sport>().FirstOrDefaultAsync(x => x.SyncId == subscription.SyncId);
                Employee? emp= await context.Set<Employee>().FirstOrDefaultAsync(x => x.SyncId == subscription.SyncId);
                Subscription? existedSubscription = await context.Set<Subscription>().FirstOrDefaultAsync(x => x.SyncId == subscription.SyncId);
                subscription.SportId = sport?.Id;
                subscription.PlayerId = player.Id;
                subscription.TrainerId = emp?.Id;
                if (existedSubscription != null)
                {
                    existedSubscription.MergeWith(subscription);
                    context.Set<Subscription>().Update(existedSubscription);
                }
                else
                {
                    EntityEntry<Subscription> CreatedResult = await context.Set<Subscription>().AddAsync(subscription);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
