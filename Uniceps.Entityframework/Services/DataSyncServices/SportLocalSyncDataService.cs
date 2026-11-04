using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.DataSyncServices
{
    public class SportLocalSyncDataService : IPushPullDataService<Sport>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public SportLocalSyncDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Sport>> GetDataToPush()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Sport>? entities = await context.Set<Sport>().Where(x => !x.IsSynced).ToListAsync();
                return entities;
            }
        }

        public async Task<long> GetLatestVersion()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<Sport>().MaxAsync(p => (long?)p.ServerVersion) ?? 0;
            }
        }

        public async Task MarkAsSynced(List<Sport> data)
        {
            if (data == null || !data.Any()) return;

            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            var syncIds = data.Select(x => x.SyncId).ToList();

            await context.Set<Sport>()
                .Where(s => syncIds.Contains(s.SyncId))
                .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.IsSynced, true));
        }

        public async Task SaveDataFromPullAsync(List<Sport> data)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            foreach (Sport sport in data)
            {
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
