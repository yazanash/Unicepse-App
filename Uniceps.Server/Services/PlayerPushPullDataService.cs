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
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Server.Services
{
    public class PlayerPushPullDataService : IPushPullDataService<Player>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PlayerPushPullDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Player>> PullDataAsync(long lastServerVersion)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Player>? entities = await context.Set<Player>().Where(x => x.ServerVersion >lastServerVersion).ToListAsync();
            return entities;
        }

        public async Task PushDataAsync(List<Player> data)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            long currentMaxVersion = await context.Set<Player>().MaxAsync(p => (long?)p.ServerVersion) ?? 0;
            foreach (Player player in data)
            {
                currentMaxVersion++;
                player.ServerVersion = currentMaxVersion;
                Player? existedPlayer = await context.Set<Player>().FirstOrDefaultAsync(x=>x.SyncId==player.SyncId);
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
