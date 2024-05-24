using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Sport;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.Services
{
   
    public class EmployeeDataService : IDataService<Employee>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public EmployeeDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }


        public async Task<Employee> CheckIfExistByName(string name)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Employee? entity = await context.Set<Employee>().AsNoTracking().FirstOrDefaultAsync((e) => e.FullName == name);
            return entity!;
        }

        public async Task<Employee> Create(Employee entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                Employee existed_sport = await CheckIfExistByName(entity.FullName!);
                if (existed_sport != null)
                    throw new ConflictException();
               foreach(Sport sport in entity.Sports!)
                {
                    context.Entry(sport).State = EntityState.Detached;
                    context.Attach(sport);
                }
                EntityEntry<Employee> CreatedResult = await context.Set<Employee>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Employee? entity = await context.Set<Employee>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            context.Set<Employee>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Employee> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Employee? entity = await context.Set<Employee>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Employee>? entities = await context.Set<Employee>().AsNoTracking().Include(x=>x.Sports).AsNoTracking().Where(x=>x.IsActive==true).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Employee>> GetAllParcentTrainers()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Employee>? entities = await context.Set<Employee>().AsNoTracking().Include(x => x.Sports).AsNoTracking().Where(x => x.IsActive == true&&x.ParcentValue>0).ToListAsync();
                return entities;
            }
        }
        public async Task<Employee> Update(Employee entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Employee existed_employee = await Get(entity.Id);
            if (existed_employee == null)
                throw new NotExistException();
            //context.Attach(entity);
            foreach (Sport sport in entity.Sports!)
            {
                context.Entry(sport).State = EntityState.Detached;
                context.Attach(sport);
            }
            context.Set<Employee>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> DeleteConnectedSports(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Employee? entity = await context.Set<Employee>().AsNoTracking().Include(x => x.Sports).AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            entity.Sports!.Clear();
            context.Set<Employee>().Update(entity!);
            await context.SaveChangesAsync();
            return true;
        }
       
    }
}
