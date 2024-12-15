using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.GetPlayerTransaction
{
    public class GetPaymentService : IGetPlayerTransactionService<PlayerPayment>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public GetPaymentService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<PlayerPayment>> GetAll(Player player)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).AsNoTracking().AsNoTracking().Include(x => x.Subscription!.Sport).AsNoTracking().Include(x => x.Subscription!.Trainer).AsNoTracking().Where(x => x.Player!.Id == player.Id && x.DataStatus != DataStatus.ToDelete).AsNoTracking()
                   .AsNoTracking().ToListAsync();
                return entities;
            }
        }
    }
}
