using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.DataSyncServices
{
    public class PlayerLocalSyncDataService : IPushPullDataService<Player>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PlayerLocalSyncDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Player>> GetDataToPush()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Player>? entities = await context.Set<Player>().Where(x => !x.IsSynced).ToListAsync();
                return entities;
            }
        }

        public async Task<long> GetLatestVersion()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<Player>().MaxAsync(p => (long?)p.ServerVersion) ?? 0;
            }
        }

        public async Task MarkAsSynced(List<Player> data)
        {
            if (data == null || !data.Any()) return;

            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            var syncIds = data.Select(x => x.SyncId).ToList();

            await context.Set<Player>()
                .Where(s => syncIds.Contains(s.SyncId))
                .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.IsSynced, true));
        }

        public async Task SaveDataFromPullAsync(List<Player> data)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            foreach (Player player in data)
            {
                Player? existedPlayer = await context.Set<Player>().FirstOrDefaultAsync(x => x.SyncId == player.SyncId);
                if (existedPlayer != null)
                {
                    existedPlayer.MergeWith(player);
                    context.Set<Player>().Update(existedPlayer);
                }
                else
                {
                    EntityEntry<Player> CreatedResult = await context.Set<Player>().AddAsync(player);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
