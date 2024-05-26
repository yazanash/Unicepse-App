using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Models.Expenses;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services
{
    public class ExpensesDataService : GenericDataService<Expenses>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public ExpensesDataService(PlatinumGymDbContextFactory contextFactory) : base(contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Expenses>> GetPeriodExpenses(DateTime pstart, DateTime pend)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Expenses>? entities = await context.Set<Expenses>().Where(x => x.date >= pstart && x.date <= pend).ToListAsync();
            return entities;

        }
        public async Task<IEnumerable<Expenses>> GetPeriodExpenses( DateTime pend)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Expenses>? entities = await context.Set<Expenses>().Where(x => x.date.Year == pend.Year && x.date.Month == pend.Month&& x.date.Day == pend.Day).ToListAsync();
            return entities;

        }
    }
}
