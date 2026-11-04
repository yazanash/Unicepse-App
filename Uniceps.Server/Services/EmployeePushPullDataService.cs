using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Sport;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Server.Services
{
    public class EmployeePushPullDataService : IPushPullDataService<Employee>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public EmployeePushPullDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Employee>> PullDataAsync(long lastServerVersion)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Employee>? entities = await context.Set<Employee>().Where(x => x.ServerVersion > lastServerVersion).ToListAsync();
            return entities;
        }

        public async Task PushDataAsync(List<Employee> data)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            long currentMaxVersion = await context.Set<Employee>().MaxAsync(p => (long?)p.ServerVersion) ?? 0;
            foreach (Employee employee in data)
            {
                currentMaxVersion++;
                employee.ServerVersion = currentMaxVersion;
                Employee? existedEmployee = await context.Set<Employee>().FirstOrDefaultAsync(x => x.SyncId == employee.SyncId);
                if (existedEmployee != null)
                {
                    existedEmployee.MergeWith(employee);
                    context.Set<Employee>().Update(existedEmployee);
                }
                else
                {
                    EntityEntry<Employee> CreatedResult = await context.Set<Employee>().AddAsync(employee);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
