using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Expenses;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.TransactionsReportServices
{
    public class ExpensesDailyReportService : IDailyTransactionService<Expenses>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public ExpensesDailyReportService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Expenses>> GetAll(DateTime date)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Expenses>? entities = await context.Set<Expenses>().Where(x => x.date.Year == date.Year && x.date.Month == date.Month && x.date.Day == date.Day).ToListAsync();
            return entities;

        }
    }
}
