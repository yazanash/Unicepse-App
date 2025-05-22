using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.TransactionsReportServices
{
    public class ExpensesPeriodReportService : IPeriodReportService<Expenses>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public ExpensesPeriodReportService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Expenses>> GetAll(DateTime dateFrom, DateTime dateTo)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Expenses>? entities = await context.Set<Expenses>().Where(x => x.date >= dateFrom && x.date <= dateTo).ToListAsync();
            return entities;
        }
    }
}
