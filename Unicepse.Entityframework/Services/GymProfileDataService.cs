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
    public class GymProfileDataService : IDataService<GymProfile>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public GymProfileDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<GymProfile> Create(GymProfile entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            EntityEntry<GymProfile> CreatedResult = await context.Set<GymProfile>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            GymProfile entityToDelete = await Get(id);
            if (entityToDelete == null)
                throw new NotExistException();
            GymProfile? entity = await context.Set<GymProfile>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<GymProfile>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<GymProfile> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            GymProfile? entity = await context.Set<GymProfile>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }
        public async Task<GymProfile?> GetByGymID(string id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            GymProfile? entity = await context.Set<GymProfile>().FirstOrDefaultAsync((e) => e.GymId == id);
            
            return entity;
        }
        public async Task<GymProfile?> Get()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            GymProfile? entity = await context.Set<GymProfile>().FirstOrDefaultAsync();
            return entity;
        }
        public async Task<IEnumerable<GymProfile>> GetAll()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<GymProfile>? entities = await context.Set<GymProfile>().ToListAsync();
            return entities;
        }

        public async Task<GymProfile> Update(GymProfile entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            GymProfile? entityToUpdate = await GetByGymID(entity.GymId!);
            if (entityToUpdate == null)
                throw new NotExistException();
            entity.Id=entityToUpdate.Id;
            context.Set<GymProfile>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
