using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.DailyActivity;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.DataSyncServices
{
    public class AttendanceDataStatusSyncService : IDataStatusService<DailyPlayerReport>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public AttendanceDataStatusSyncService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<DailyPlayerReport>> GetByDataStatus(DataStatus status)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<DailyPlayerReport>? entities = await context.Set<DailyPlayerReport>().Include(x => x.Player).Where(x => x.DataStatus == status).ToListAsync();
                return entities;
            }
        }

        public async Task<DailyPlayerReport> UpdateDataStatus(DailyPlayerReport entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            DailyPlayerReport? existedPlayer = await context.DailyPlayerReport!.FindAsync(entity.Id);
            if (existedPlayer == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Entry(entity).Property(e => e.DataStatus).IsModified = true;
            await context.SaveChangesAsync();
            return entity;

        }
    }
}
