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
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.Entityframework.Services.RoutineSystemServices
{
    public class RoutineItemDataService : IDataService<RoutineItemModel>, IGetAllById<RoutineItemModel>, IUpdateRangeDataService<RoutineItemModel>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public RoutineItemDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<RoutineItemModel> Create(RoutineItemModel entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            context.Attach(entity.Day!);
            context.Attach(entity.Exercise!);
            EntityEntry<RoutineItemModel> CreatedResult = await context.Set<RoutineItemModel>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            RoutineItemModel entityToDelete = await Get(id);
            if (entityToDelete == null)
                throw new NotExistException("هذا السجل غير موجود");
            RoutineItemModel? entity = await context.Set<RoutineItemModel>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<RoutineItemModel>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<RoutineItemModel> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            RoutineItemModel? entity = await context.Set<RoutineItemModel>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<RoutineItemModel>> GetAll()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<RoutineItemModel>? entities = await context.Set<RoutineItemModel>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<RoutineItemModel>> GetAllById(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<RoutineItemModel>? entities = await context.Set<RoutineItemModel>().Include(x=>x.Sets).Where(x => x.DayId == id)
                .Include(x => x.Exercise).ToListAsync();
            return entities;
        }

        public async Task<RoutineItemModel> Update(RoutineItemModel entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            RoutineItemModel? entityToUpdate = await context.Set<RoutineItemModel>().Include(x=>x.Sets).FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (entityToUpdate == null)
                throw new NotExistException("هذا السجل غير موجود");
            entityToUpdate.Order = entity.Order;
            context.Set<RoutineItemModel>().Update(entityToUpdate);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<RoutineItemModel>> UpdateRange(List<RoutineItemModel> entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            foreach(var item in entity)
            {
                RoutineItemModel? entityToUpdate = await context.Set<RoutineItemModel>().Include(x => x.Sets).FirstOrDefaultAsync(x => x.Id == item.Id);
                if (entityToUpdate == null)
                    throw new NotExistException("هذا السجل غير موجود");
                entityToUpdate.Order = item.Order;
                context.Set<RoutineItemModel>().Update(entityToUpdate);
            }
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
