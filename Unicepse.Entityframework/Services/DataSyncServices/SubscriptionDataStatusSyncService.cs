using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.DataSyncServices
{
    public class SubscriptionDataStatusSyncService : IDataStatusService<Subscription>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public SubscriptionDataStatusSyncService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Subscription>> GetByDataStatus(DataStatus status)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().Where(x => x.DataStatus == status).Include(x => x.Trainer)
                    .Include(x => x.Player).AsNoTracking().Include(x => x.Sport!.Trainers).AsNoTracking()
                    .Include(x => x.Sport).AsNoTracking().ToListAsync();
                return entities;
            }
        }

        public async Task<Subscription> UpdateDataStatus(Subscription entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Subscription? dataToSync = await context.Subscriptions!.FindAsync(entity.Id);
            if (dataToSync == null)
                throw new NotExistException("هذا السجل غير موجود");
            dataToSync.DataStatus = entity.DataStatus;
            context.Entry(dataToSync).Property(e => e.DataStatus).IsModified = true;
            await context.SaveChangesAsync();
            return entity;

        }
    }
}
