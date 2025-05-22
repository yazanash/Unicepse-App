using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.GetPlayerTransaction
{
    public class GetPaymentService : IGetPlayerTransactionService<PlayerPayment>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public GetPaymentService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<PlayerPayment>> GetAll(Player player)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).AsNoTracking().AsNoTracking().Include(x => x.Subscription!.Sport).AsNoTracking().Include(x => x.Subscription!.Trainer).AsNoTracking().Where(x => x.Player!.Id == player.Id && x.DataStatus != DataStatus.ToDelete).AsNoTracking()
                   .AsNoTracking().ToListAsync();
                return entities;
            }
        }
    }
}
