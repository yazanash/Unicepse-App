using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class EmployeeCreditsDataService : IDataService<Credit>, IEmployeeTransaction<Credit>, IEmployeeMonthlyTransaction<Credit>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public EmployeeCreditsDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Credit> Create(Credit entity)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                if (entity.EmpPerson != null)
                {
                    entity.EmpPersonId = entity.EmpPerson.Id;
                    entity.EmpPerson = null;
                }
                EntityEntry<Credit> CreatedResult = await context.Set<Credit>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Credit? entity = await context.Set<Credit>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الدفعة غير موجودة");
            context.Set<Credit>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Credit> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Credit? entity = await context.Set<Credit>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الدفعة غير موجودة");
            return entity!;
        }

        public async Task<IEnumerable<Credit>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().AsNoTracking().ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Credit>> GetAll(Employee trainer)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().Where(x => x.EmpPerson!.Id == trainer.Id).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Credit>> GetAll(Employee trainer, DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().Include(x => x.EmpPerson).AsNoTracking().Where(x => x.EmpPerson!.Id == trainer.Id && x.Date.Year == date.Year && x.Date.Month == date.Month).ToListAsync();
                return entities;
            }
        }
        public async Task<Credit> Update(Credit entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Credit existed_employee = await Get(entity.Id);
            if (existed_employee == null)
                throw new NotExistException("هذه الدفعة غير موجودة");
            context.Set<Credit>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
