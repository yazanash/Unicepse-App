using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Models.Expenses;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;

namespace PlatinumGym.Entityframework.Services
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
    }
}
