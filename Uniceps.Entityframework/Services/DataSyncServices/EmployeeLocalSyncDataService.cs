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

namespace Uniceps.Entityframework.Services.DataSyncServices
{
    public class EmployeeLocalSyncDataService : IPushPullDataService<Employee>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public EmployeeLocalSyncDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Employee>> GetDataToPush()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Employee>? entities = await context.Set<Employee>().Where(x => !x.IsSynced).ToListAsync();
                return entities;
            }
        }

        public async Task<long> GetLatestVersion()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<Employee>().MaxAsync(p => (long?)p.ServerVersion) ?? 0;
            }
        }

        public async Task MarkAsSynced(List<Employee> data)
        {
            if (data == null || !data.Any()) return;

            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            var syncIds = data.Select(x => x.SyncId).ToList();

            await context.Set<Employee>()
                .Where(s => syncIds.Contains(s.SyncId))
                .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.IsSynced, true));
        }

        public async Task SaveDataFromPullAsync(List<Employee> data)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            foreach (Employee employee in data)
            {
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
