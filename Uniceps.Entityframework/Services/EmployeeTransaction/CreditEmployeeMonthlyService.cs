using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.EmployeeTransaction
{
    public class CreditEmployeeMonthlyService : IEmployeeMonthlyTransaction<Credit>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public CreditEmployeeMonthlyService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Credit>> GetAll(Employee trainer, DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().Include(x => x.EmpPerson).AsNoTracking().Where(x => x.EmpPerson!.Id == trainer.Id && x.Date.Year == date.Year && x.Date.Month == date.Month).ToListAsync();
                return entities;
            }
        }
    }
}
