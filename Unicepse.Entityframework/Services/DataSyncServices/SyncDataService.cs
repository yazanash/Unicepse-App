using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.SyncModel;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.DataSyncServices
{
    public class SyncDataService : IDataService<SyncObject>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public SyncDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<SyncObject> Create(SyncObject entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<SyncObject> CreatedResult = await context.Set<SyncObject>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            SyncObject? entity = await context.Set<SyncObject>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<SyncObject>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<SyncObject> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            SyncObject? entity = await context.Set<SyncObject>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }

        public async Task<IEnumerable<SyncObject>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<SyncObject>? entities = await context.Set<SyncObject>().ToListAsync();
                return entities;
            }
        }

        public async Task<SyncObject> Update(SyncObject entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            SyncObject existed_employee = await Get(entity.Id);
            if (existed_employee == null)
                throw new NotExistException("هذا السجل غير موجود");
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
