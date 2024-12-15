﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Metric;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;

namespace Unicepse.Entityframework.Services
{
    public class MetricDataService : IDataService<Metric>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public MetricDataService(PlatinumGymDbContextFactory contextFactory) 
        {
            _contextFactory = contextFactory;
        }

        public async Task<Metric> Create(Metric entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            context.Attach(entity.Player!);
            //context.Attach(entity.Subscription!.Player!);
            //context.tity.Player!);
            EntityEntry<Metric> CreatedResult = await context.Set<Metric>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Metric? entity = await context.Set<Metric>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<Metric>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Metric> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Metric? entity = await context.Set<Metric>().Include(x => x.Player).FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<Metric>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Metric>? entities = await context.Set<Metric>().Include(x => x.Player)
                    .ToListAsync();
                return entities;
            }
        }
     
       
        public async Task<Metric> Update(Metric entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Metric existed_payment = await Get(entity.Id);
            if (existed_payment == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<Metric>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
