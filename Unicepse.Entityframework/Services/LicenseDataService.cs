using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services
{
    public class LicenseDataService : IDataService<License>
    {

        private readonly PlatinumGymDbContextFactory _contextFactory;

        public LicenseDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<License> Create(License entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            EntityEntry<License> CreatedResult = await context.Set<License>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            License entityToDelete = await Get(id);
            if (entityToDelete == null)
                throw new NotExistException("هذا السجل غير موجود");
            License? entity = await context.Set<License>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<License>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<License> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            License? entity = await context.Set<License>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }
        public async Task<License> GetById(string id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            License? entity = await context.Set<License>().FirstOrDefaultAsync((e) => e.LicenseId == id);
            return entity!;
        }

        public async Task<IEnumerable<License>> GetAll()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<License>? entities = await context.Set<License>().ToListAsync();
            return entities;
        }
        public License? ActiveLicenses()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            License? entities = context.Set<License>().Where(x=>x.SubscribeEndDate>=DateTime.Now).FirstOrDefault();
            return entities;
        }
        public async Task<License> Update(License entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            License entityToUpdate = await GetById(entity.LicenseId!);
            if (entityToUpdate == null)
                throw new NotExistException("هذا السجل غير موجود");
            entity.Id = entityToUpdate.Id;
            context.Set<License>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
