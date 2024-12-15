using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.DataSyncServices
{
    public class RoutineDataStatusSyncService : IDataStatusService<PlayerRoutine>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public RoutineDataStatusSyncService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PlayerRoutine> UpdateDataStatus(PlayerRoutine entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine? dataToSync = await context.PlayerRoutine!.FindAsync(entity.Id);
            if (dataToSync == null)
                throw new NotExistException("هذا السجل غير موجود");
            dataToSync.DataStatus = entity.DataStatus;
            context.Entry(dataToSync).Property(e => e.DataStatus).IsModified = true;
            await context.SaveChangesAsync();
            return entity;

        }
        public async Task<IEnumerable<PlayerRoutine>> GetByDataStatus(DataStatus status)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerRoutine>? entities = await context.Set<PlayerRoutine>().Include(x => x.Player).AsNoTracking().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercises).AsNoTracking().Where(x => x.DataStatus == status).ToListAsync();
                return entities;
            }
        }
    }
}
