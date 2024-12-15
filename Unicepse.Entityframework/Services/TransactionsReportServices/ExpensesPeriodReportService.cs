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
    public class ExpensesPeriodReportService : IPeriodReportService<Expenses>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public ExpensesPeriodReportService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Expenses>> GetAll(DateTime dateFrom, DateTime dateTo)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Expenses>? entities = await context.Set<Expenses>().Where(x => x.date >= dateFrom && x.date <= dateTo).ToListAsync();
            return entities;
        }
    }
}
