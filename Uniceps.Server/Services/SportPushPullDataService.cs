using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Server.Services
{
    public class SportPushPullDataService : IPushPullDataService<Sport>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public SportPushPullDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Sport>> PullDataAsync(long lastServerVersion)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Sport>? entities = await context.Set<Sport>().Where(x => x.ServerVersion > lastServerVersion).ToListAsync();
            return entities;
        }

        public async Task PushDataAsync(List<Sport> data)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            long currentMaxVersion = await context.Set<Sport>().MaxAsync(p => (long?)p.ServerVersion) ?? 0;
            foreach (Sport sport in data)
            {
                currentMaxVersion++;
                sport.ServerVersion = currentMaxVersion;
                Sport? existedSport = await context.Set<Sport>().FirstOrDefaultAsync(x => x.SyncId == sport.SyncId);
                if (existedSport != null)
                {
                    existedSport.MergeWith(sport);
                    context.Set<Sport>().Update(existedSport);
                }
                else
                {
                    EntityEntry<Sport> CreatedResult = await context.Set<Sport>().AddAsync(sport);

                }
            }
            await context.SaveChangesAsync();
        }
    }
}
