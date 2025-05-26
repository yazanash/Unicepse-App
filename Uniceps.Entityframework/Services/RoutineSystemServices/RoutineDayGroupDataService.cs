using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Core.Exceptions;

namespace Uniceps.Entityframework.Services.RoutineSystemServices
{
    public class RoutineDayGroupDataService : IDataService<DayGroup>, IGetAllById<DayGroup>,IUpdateRangeDataService<DayGroup>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public RoutineDayGroupDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<DayGroup> Create(DayGroup entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            context.Attach(entity.Routine!);
            EntityEntry<DayGroup> CreatedResult = await context.Set<DayGroup>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            DayGroup entityToDelete = await Get(id);
            if (entityToDelete == null)
                throw new NotExistException("هذا السجل غير موجود");
            DayGroup? entity = await context.Set<DayGroup>().FirstOrDefaultAsync((e) => e.Id == id);

            context.Set<DayGroup>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<DayGroup> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            DayGroup? entity = await context.Set<DayGroup>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<DayGroup>> GetAll()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<DayGroup>? entities = await context.Set<DayGroup>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<DayGroup>> GetAllById(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<DayGroup>? entities = await context.Set<DayGroup>().Where(x => x.RoutineId == id).ToListAsync();
            return entities;
        }

        public async Task<DayGroup> Update(DayGroup entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            DayGroup entityToUpdate = await Get(entity.Id);
            if (entityToUpdate == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<DayGroup>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<DayGroup>> UpdateRange(List<DayGroup> entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            context.Set<DayGroup>().UpdateRange(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
