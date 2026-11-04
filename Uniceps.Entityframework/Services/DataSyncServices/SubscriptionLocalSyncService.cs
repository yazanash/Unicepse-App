using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.DataSyncServices
{
    public class SubscriptionLocalSyncService : IPushPullDataService<Subscription>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public SubscriptionLocalSyncService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Subscription>> GetDataToPush()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().Where(x => !x.IsSynced).ToListAsync();
                return entities;
            }
        }

        public async Task<long> GetLatestVersion()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<Subscription>().MaxAsync(p => (long?)p.ServerVersion) ?? 0;
            }
        }

        public async Task MarkAsSynced(List<Subscription> data)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            foreach (Subscription entity in data)
            {
                await context.Set<Subscription>()
                 .Where(s => s.SyncId == entity.SyncId)
                 .ExecuteUpdateAsync(setter => setter
                    .SetProperty(s => s.IsSynced, entity.IsSynced));
            }
        }

        public async Task SaveDataFromPullAsync(List<Subscription> data)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            foreach (Subscription player in data)
            {
                Subscription? existedPlayer = await context.Set<Subscription>().FirstOrDefaultAsync(x => x.SyncId == player.SyncId);
                if (existedPlayer != null)
                {
                    existedPlayer.MergeWith(player);
                    context.Set<Subscription>().Update(existedPlayer);
                }
                else
                {
                    EntityEntry<Subscription> CreatedResult = await context.Set<Subscription>().AddAsync(player);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
