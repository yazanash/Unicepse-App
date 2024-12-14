using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.GetPlayerTransaction
{
    public class GetSubscriptionsService : IGetPlayerTransactionService<Subscription>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public GetSubscriptionsService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Subscription>> GetAll(Player player)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.Player!.Id == player.Id).Include(x => x.Trainer)
                    .Include(x => x.Player).AsNoTracking().Include(x => x.Sport!.Trainers).AsNoTracking()
                    .Include(x => x.Sport).AsNoTracking().Include(x => x.Payments).AsNoTracking().ToListAsync();
                return entities;
            }
        }
    }
}
