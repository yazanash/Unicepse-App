using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Metric;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.GetPlayerTransaction
{
    public class GetMetricsService : IGetPlayerTransactionService<Metric>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public GetMetricsService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Metric>> GetAll(Player player)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Metric>? entities = await context.Set<Metric>().Where(x => x.Player!.Id == player.Id).Include(x => x.Player)
                    .ToListAsync();
                return entities;
            }
        }

    }
}
