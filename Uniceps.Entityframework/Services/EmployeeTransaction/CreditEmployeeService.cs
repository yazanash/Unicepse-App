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
    public class CreditEmployeeService : IEmployeeTransaction<Credit>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public CreditEmployeeService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Credit>> GetAll(Employee trainer)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().Where(x => x.EmpPerson!.Id == trainer.Id).ToListAsync();
                return entities;
            }
        }
    }
}
