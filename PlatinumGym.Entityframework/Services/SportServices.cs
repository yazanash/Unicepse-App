using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Models;
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
    public class SportServices : IDataService<Sport>
    {

        private readonly PlatinumGymDbContextFactory _contextFactory;

        public SportServices(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public Task<Sport> CheckIfExistByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Sport> Create(Sport entity,List<Employee> trainers)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                foreach (Employee emp in trainers)
                {
                    var trainer = context.Employees?.Find(emp.Id);
                    if(trainer != null)
                    entity.Trainers!.Add(trainer);
                }
                EntityEntry<Sport> CreatedResult = await context.Set<Sport>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }
        public async Task<Sport> Create(Sport entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<Sport> CreatedResult = await context.Set<Sport>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }
        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Sport> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Sport>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Sport>? entities = await context.Set<Sport>().Include(x => x.Trainers).ToListAsync();
                return entities;
            }
        }

        public Task<IEnumerable<Sport>> GetByFilterAll(Filter filter)
        {
            throw new NotImplementedException();
        }

        public Task<Sport> Update(Sport entity)
        {
            throw new NotImplementedException();
        }
    }
}
