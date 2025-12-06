using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public GenericDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            EntityEntry<T> CreatedResult = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            T entityToDelete = await Get(id);
            if (entityToDelete == null)
                throw new NotExistException("هذا السجل غير موجود");
            T? entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<T>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<T> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            T? entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<T>? entities = await context.Set<T>().ToListAsync();
            return entities;

        }

        public async Task<T> Update(T entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            T entityToUpdate = await Get(entity.Id);
            if (entityToUpdate == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;

        }
    }
}
