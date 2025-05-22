using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Core.Exceptions;

namespace Uniceps.Entityframework.Services
{
    public class LicenseDataService : IDataService<License>
    {

        private readonly UnicepsDbContextFactory _contextFactory;
        private readonly IPublicIdService<License> _publicIdService;


        public LicenseDataService(UnicepsDbContextFactory contextFactory, IPublicIdService<License> publicIdService)
        {
            _contextFactory = contextFactory;
            _publicIdService = publicIdService;
        }

        public async Task<License> Create(License entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            EntityEntry<License> CreatedResult = await context.Set<License>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
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
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            License? entity = await context.Set<License>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }


        public async Task<IEnumerable<License>> GetAll()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<License>? entities = await context.Set<License>().ToListAsync();
            return entities;
        }
        public async Task<License> Update(License entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            License? entityToUpdate = await _publicIdService.GetByUID(entity.LicenseId!);
            if (entityToUpdate == null)
                throw new NotExistException("هذا السجل غير موجود");
            entity.Id = entityToUpdate.Id;
            context.Set<License>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
