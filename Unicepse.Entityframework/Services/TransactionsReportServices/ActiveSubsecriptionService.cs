using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.TransactionsReportServices
{
    public class ActiveSubsecriptionService : IActiveTransactionService<Subscription>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public ActiveSubsecriptionService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Subscription>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().Where(x => x.EndDate >= DateTime.Now).Include(x => x.Trainer).AsNoTracking()
                    .Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Sport).AsNoTracking().ToListAsync();
                return entities;
            }
        }
    }
}
