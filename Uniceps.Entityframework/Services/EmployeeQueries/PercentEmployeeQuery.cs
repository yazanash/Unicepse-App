using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.EmployeeQueries
{
    public class PercentEmployeeQuery : IEmployeeQuery
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PercentEmployeeQuery(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Employee>? entities = await context.Set<Employee>().AsNoTracking().Include(x => x.Sports).AsNoTracking().Where(x => x.IsActive == true && x.ParcentValue > 0).ToListAsync();
                return entities;
            }
        }
    }
}
