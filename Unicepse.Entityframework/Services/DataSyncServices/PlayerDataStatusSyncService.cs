using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.DataSyncServices
{
    public class PlayerDataStatusSyncService : IDataStatusService<Player>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public PlayerDataStatusSyncService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Player>> GetByDataStatus(DataStatus status)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Player>? entities = await context.Set<Player>().Where(x => x.DataStatus == status).ToListAsync();
                return entities;
            }
        }

        public async Task<Player> UpdateDataStatus(Player entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Player? dataToSync = await context.Players!.FindAsync(entity.Id);
            if (dataToSync == null)
                throw new PlayerNotExistException("هذا اللاعب غير موجود");

            dataToSync.DataStatus = entity.DataStatus;
            context.Entry(dataToSync).Property(e => e.DataStatus).IsModified = true;
            await context.SaveChangesAsync();
            return entity;

        }
    }
}
