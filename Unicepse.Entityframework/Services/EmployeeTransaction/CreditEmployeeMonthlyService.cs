using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.EmployeeTransaction
{
    public class CreditEmployeeMonthlyService : IEmployeeMonthlyTransaction<Credit>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public CreditEmployeeMonthlyService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Credit>> GetAll(Employee trainer, DateTime date)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().Include(x => x.EmpPerson).AsNoTracking().Where(x => x.EmpPerson!.Id == trainer.Id && x.Date.Year == date.Year && x.Date.Month == date.Month).ToListAsync();
                return entities;
            }
        }
    }
}
