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
using Uniceps.Core.Models.Sport;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{

    public class EmployeeDataService : IDataService<Employee>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public EmployeeDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }


        public async Task<Employee> CheckIfExistByName(string name)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Employee? entity = await context.Set<Employee>().AsNoTracking().FirstOrDefaultAsync((e) => e.FullName == name);
            return entity!;
        }

        public async Task<Employee> Create(Employee entity)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                var exists = await context.Set<Employee>().AnyAsync(x => x.FullName == entity.FullName);
                if (exists) throw new ConflictException("الموظف موجود مسبقاً");

                Employee entityToCreate = new();
                entityToCreate.MergeWith(entity);

                entityToCreate.Sports = new List<Sport>();

                await context.Set<Employee>().AddAsync(entityToCreate);
                await context.SaveChangesAsync();

                if (entity.Sports?.Count > 0)
                {
                    var sportIds = entity.Sports.Select(s => s.Id).ToList();
                    var existingSports = await context.Set<Sport>()
                        .Where(s => sportIds.Contains(s.Id))
                        .ToListAsync();

                    foreach (var sport in existingSports)
                    {
                        entityToCreate.Sports.Add(sport);
                    }

                    await context.SaveChangesAsync();
                }
                 
                return entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Employee? entity = await context.Set<Employee>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا الموظف غير موجود");
            context.Set<Employee>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Employee> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Employee? entity = await context.Set<Employee>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Employee>? entities = await context.Set<Employee>().AsNoTracking().Include(x => x.Sports).AsNoTracking().Where(x => x.IsActive == true).ToListAsync();
                return entities;
            }
        }
        public async Task<Employee> Update(Employee entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Employee existed_employee = await Get(entity.Id);
            if (existed_employee == null)
                throw new NotExistException("هذا الموظف غير موجود");
            //context.Attach(entity);
            foreach (Sport sport in entity.Sports!)
            {
                context.Entry(sport).State = EntityState.Detached;
                if (context.Entry(sport).State == EntityState.Detached)
                    context.Entry(sport).State = EntityState.Modified;
            }

            context.Entry(entity).State = EntityState.Detached;
            if (context.Entry(entity).State == EntityState.Detached)
                context.Entry(entity).State = EntityState.Modified;
            context.Set<Employee>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

    }
}
