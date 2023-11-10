using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Employee;
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
            Employee? entity = await context.Set<Employee>().FirstOrDefaultAsync((e) => e.FullName == name);
            return entity;
        }

        public async Task<Employee> Create(Employee entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                Employee existed_sport = await CheckIfExistByName(entity.FullName!);
                if (existed_sport != null)
                    throw new ConflictException();
                EntityEntry<Employee> CreatedResult = await context.Set<Employee>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Employee> Update(Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}
