using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.PlayerQueries
{
    public class ArchivedPlayerService : IArchivedService<Player>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public ArchivedPlayerService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Player>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Player>? entities = await context.Set<Player>().Where(x => x.IsSubscribed == false).ToListAsync();
                return entities;
            }
        }
    }
}
