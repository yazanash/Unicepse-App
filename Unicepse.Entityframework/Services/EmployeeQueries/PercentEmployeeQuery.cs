using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.EmployeeQueries
{
    public class PercentEmployeeQuery : IEmployeeQuery
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public PercentEmployeeQuery(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Employee>? entities = await context.Set<Employee>().AsNoTracking().Include(x => x.Sports).AsNoTracking().Where(x => x.IsActive == true && x.ParcentValue > 0).ToListAsync();
                return entities;
            }
        }
    }
}
