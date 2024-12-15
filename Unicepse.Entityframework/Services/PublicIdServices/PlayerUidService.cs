using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.PublicIdServices
{
    public class PlayerUidService : IPublicIdService<Player>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public PlayerUidService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Player?> GetByUID(string uid)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.UID == uid);
            return entity!;
        }
    }
}
