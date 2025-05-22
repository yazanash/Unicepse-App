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
    public class GymProfileDataService : IDataService<GymProfile>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        private readonly IPublicIdService<GymProfile> _publicIdService;
        public GymProfileDataService(UnicepsDbContextFactory contextFactory, IPublicIdService<GymProfile> publicIdService)
        {
            _contextFactory = contextFactory;
            _publicIdService = publicIdService;
        }
        public async Task<GymProfile> Create(GymProfile entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            EntityEntry<GymProfile> CreatedResult = await context.Set<GymProfile>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            GymProfile entityToDelete = await Get(id);
            if (entityToDelete == null)
                throw new NotExistException("هذا السجل غير موجود");
            GymProfile? entity = await context.Set<GymProfile>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<GymProfile>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<GymProfile> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            GymProfile? entity = await context.Set<GymProfile>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }
        public async Task<IEnumerable<GymProfile>> GetAll()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<GymProfile>? entities = await context.Set<GymProfile>().ToListAsync();
            return entities;
        }

        public async Task<GymProfile> Update(GymProfile entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            GymProfile? entityToUpdate = await _publicIdService.GetByUID(entity.GymId!);
            if (entityToUpdate == null)
                throw new NotExistException("هذا السجل غير موجود");
            entity.Id = entityToUpdate.Id;
            context.Set<GymProfile>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
