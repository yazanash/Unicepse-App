using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Core.Exceptions;

namespace Uniceps.Entityframework.Services.DataSyncServices
{
    public class SyncDataService : IDataService<SyncObject>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public SyncDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<SyncObject> Create(SyncObject entity)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<SyncObject> CreatedResult = await context.Set<SyncObject>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            SyncObject? entity = await context.Set<SyncObject>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<SyncObject>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<SyncObject> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            SyncObject? entity = await context.Set<SyncObject>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }

        public async Task<IEnumerable<SyncObject>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<SyncObject>? entities = await context.Set<SyncObject>().ToListAsync();
                return entities;
            }
        }

        public async Task<SyncObject> Update(SyncObject entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            SyncObject existed_employee = await Get(entity.Id);
            if (existed_employee == null)
                throw new NotExistException("هذا السجل غير موجود");
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
