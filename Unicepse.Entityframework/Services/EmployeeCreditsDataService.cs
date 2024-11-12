using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Entityframework.Services
{
    public class EmployeeCreditsDataService : ICreditDataService
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public EmployeeCreditsDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Credit> Create(Credit entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<Credit> CreatedResult = await context.Set<Credit>().AddAsync(entity);
                if (entity.EmpPerson != null)
                    context.Attach(entity.EmpPerson);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Credit? entity = await context.Set<Credit>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الدفعة غير موجودة");
            context.Set<Credit>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Credit>> GetAll(Employee trainer)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().Where(x=>x.EmpPerson!.Id==trainer.Id).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Credit>> GetAll(Employee trainer, DateTime date)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().Include(x=>x.EmpPerson).AsNoTracking().Where(x => x.EmpPerson!.Id == trainer.Id&& x.Date.Year==date.Year&&x.Date.Month==date.Month).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Credit>> GetAll( DateTime date)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().Include(x => x.EmpPerson).AsNoTracking().Where(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day).ToListAsync();
                return entities;
            }
        }
        public async Task<Credit> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Credit? entity = await context.Set<Credit>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الدفعة غير موجودة");
            return entity!;
        }

        public async Task<IEnumerable<Credit>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Credit>? entities = await context.Set<Credit>().AsNoTracking().AsNoTracking().ToListAsync();
                return entities;
            }
        }

        public async Task<Credit> Update(Credit entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Credit existed_employee = await Get(entity.Id);
            if (existed_employee == null)
                throw new NotExistException("هذه الدفعة غير موجودة");
            context.Set<Credit>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
