using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class OtherRevenuesDataService : IDataService<OtherRevenue>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public OtherRevenuesDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<OtherRevenue> Create(OtherRevenue entity)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                await context.Set<OtherRevenue>().AddAsync(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            OtherRevenue? entity = await context.Set<OtherRevenue>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا القيد غير موجود");
            context.Set<OtherRevenue>().Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<OtherRevenue> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            OtherRevenue? entity = await context.Set<OtherRevenue>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }

        public async Task<IEnumerable<OtherRevenue>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<OtherRevenue>? entities = await context.Set<OtherRevenue>().ToListAsync();
                return entities;
            }
        }

        public async Task<OtherRevenue> Update(OtherRevenue entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            OtherRevenue? existEntity = await context.Set<OtherRevenue>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == entity.Id);
            if (existEntity == null)
                throw new NotExistException();

            context.Set<OtherRevenue>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
