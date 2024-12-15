using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.TransactionsReportServices
{
    public class CreditsDailyReportService : IDailyTransactionService<Credit>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public CreditsDailyReportService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Credit>> GetAll(DateTime date)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().Include(x => x.EmpPerson).AsNoTracking().Where(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day).ToListAsync();
                return entities;
            }
        }
    }
}
