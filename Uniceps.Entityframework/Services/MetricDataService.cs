using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Metric;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class MetricDataService : IDataService<Metric>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public MetricDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Metric> Create(Metric entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            context.Attach(entity.Player!);
            //context.Attach(entity.Subscription!.Player!);
            //context.tity.Player!);
            EntityEntry<Metric> CreatedResult = await context.Set<Metric>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Metric? entity = await context.Set<Metric>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<Metric>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Metric> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Metric? entity = await context.Set<Metric>().Include(x => x.Player).FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<Metric>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Metric>? entities = await context.Set<Metric>().Include(x => x.Player)
                    .ToListAsync();
                return entities;
            }
        }


        public async Task<Metric> Update(Metric entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Metric existed_payment = await Get(entity.Id);
            if (existed_payment == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<Metric>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
