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
    public class ExpensesDailyReportService : IDailyTransactionService<Expenses>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public ExpensesDailyReportService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Expenses>> GetAll(DateTime date)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Expenses>? entities = await context.Set<Expenses>().Where(x => x.date.Year == date.Year && x.date.Month == date.Month && x.date.Day == date.Day).ToListAsync();
            return entities;

        }
    }
}
