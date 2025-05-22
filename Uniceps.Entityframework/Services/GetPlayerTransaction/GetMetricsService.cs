using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.GetPlayerTransaction
{
    public class GetMetricsService : IGetPlayerTransactionService<Metric>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public GetMetricsService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Metric>> GetAll(Player player)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Metric>? entities = await context.Set<Metric>().Where(x => x.Player!.Id == player.Id).Include(x => x.Player)
                    .ToListAsync();
                return entities;
            }
        }

    }
}
