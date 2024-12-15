using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Metric;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.DataSyncServices
{
    public class MetricDataStatusSyncService : IDataStatusService<Metric>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public MetricDataStatusSyncService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Metric>> GetByDataStatus(DataStatus status)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Metric>? entities = await context.Set<Metric>().Where(x => x.DataStatus == status).Include(x => x.Player).AsNoTracking().ToListAsync();
                return entities;
            }
        }

        public async Task<Metric> UpdateDataStatus(Metric entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Metric? dataToSync = await context.Metrics!.FindAsync(entity.Id);
            if (dataToSync == null)
                throw new NotExistException("هذا السجل غير موجود");
            dataToSync.DataStatus = entity.DataStatus;
            context.Entry(dataToSync).Property(e => e.DataStatus).IsModified = true;
            await context.SaveChangesAsync();
            return entity;

        }
    }
}
