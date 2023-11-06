using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Models;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.Services
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public GenericDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            EntityEntry<T> CreatedResult = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            T? entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<T>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<T> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
                T? entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                return entity!;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            
                IEnumerable<T>? entities = await context.Set<T>().ToListAsync();
                return entities;
            
        }

        public Task<IEnumerable<T>> GetByFilterAll(Filter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Update(T entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            
        }
    }
}
