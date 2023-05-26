using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGymPro.DbContexts;
using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Services
{
    public class SportServices : IDataService<Sport>
    {

        private readonly PlatinumGymDbContextFactory _contextFactory;

        public SportServices(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
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

        public Task<Sport> Update(Sport entity)
        {
            throw new NotImplementedException();
        }
    }
}
