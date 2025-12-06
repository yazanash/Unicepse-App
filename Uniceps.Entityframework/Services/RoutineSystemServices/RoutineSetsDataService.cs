using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Core.Exceptions;

namespace Uniceps.Entityframework.Services.RoutineSystemServices
{
    public class RoutineSetsDataService : IDataService<SetModel>, IGetAllById<SetModel>, IUpdateRangeDataService<SetModel>, IApplySetsToAll
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public RoutineSetsDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<SetModel> Create(SetModel entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            EntityEntry<SetModel> CreatedResult = await context.Set<SetModel>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            SetModel entityToDelete = await Get(id);
            if (entityToDelete == null)
                throw new NotExistException("هذا السجل غير موجود");
            SetModel? entity = await context.Set<SetModel>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<SetModel>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<SetModel> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            SetModel? entity = await context.Set<SetModel>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<SetModel>> GetAll()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<SetModel>? entities = await context.Set<SetModel>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<SetModel>> GetAllById(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<SetModel>? entities = await context.Set<SetModel>().Where(x => x.RoutineItemId == id).ToListAsync();
            return entities;
        }

        public async Task<SetModel> Update(SetModel entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            SetModel entityToUpdate = await Get(entity.Id);
            if (entityToUpdate == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<SetModel>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<List<SetModel>> ApplySetsToEntity(List<SetModel> entities,int itemId)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            List<SetModel> oldsets = await context.Set<SetModel>().Where(s => s.RoutineItemId == itemId).ToListAsync();
            context.Set<SetModel>().RemoveRange(oldsets);
            await context.Set<SetModel>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
            return entities;
        }
        public async Task<IEnumerable<SetModel>> UpdateRange(List<SetModel> entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            
            context.Set<SetModel>().UpdateRange(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
