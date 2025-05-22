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
    public class RoutineModelDataService : IDataService<RoutineModel>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public RoutineModelDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<RoutineModel> Create(RoutineModel entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            EntityEntry<RoutineModel> CreatedResult = await context.Set<RoutineModel>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            RoutineModel entityToDelete = await Get(id);
            if (entityToDelete == null)
                throw new NotExistException("هذا السجل غير موجود");
            RoutineModel? entity = await context.Set<RoutineModel>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<RoutineModel>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<RoutineModel> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            RoutineModel? entity = await context.Set<RoutineModel>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<RoutineModel>> GetAll()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<RoutineModel>? entities = await context.Set<RoutineModel>().ToListAsync();
            return entities;
        }

        public async Task<RoutineModel> Update(RoutineModel entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            RoutineModel entityToUpdate = await Get(entity.Id);
            if (entityToUpdate == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<RoutineModel>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
