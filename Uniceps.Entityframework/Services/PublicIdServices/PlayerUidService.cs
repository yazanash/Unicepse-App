using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.PublicIdServices
{
    public class PlayerUidService : IPublicIdService<Player>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PlayerUidService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Player?> GetByUID(string uid)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.UID == uid);
            return entity!;
        }
    }
}
