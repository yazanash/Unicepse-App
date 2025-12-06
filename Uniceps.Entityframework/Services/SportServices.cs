using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Sport;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class SportServices : IDataService<Sport>
    {

        private readonly UnicepsDbContextFactory _contextFactory;

        public SportServices(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Sport?> CheckIfExistByName(string name)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Sport? entity = await context.Set<Sport>().AsNoTracking().FirstOrDefaultAsync((e) => e.Name == name);
            return entity;
        }

        //public async Task<Sport> Create(Sport entity,List<Employee> trainers)
        //{
        //    using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
        //    {
        //        Sport? existed_sport = await CheckIfExistByName(entity.Name!);
        //        if (existed_sport != null)
        //            throw new SportConflictException();
        //        foreach (Employee emp in trainers)
        //        {
        //            var trainer = context.Employees?.Find(emp.Id);
        //            if(trainer != null)
        //            entity.Trainers!.Add(trainer);
        //        }
        //        EntityEntry<Sport> CreatedResult = await context.Set<Sport>().AddAsync(entity);
        //        await context.SaveChangesAsync();
        //        return CreatedResult.Entity;
        //    }
        //}
        public async Task<Sport> Create(Sport entity)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                //Sport? existed_sport = await CheckIfExistByName(entity.Name!);
                //if (existed_sport != null)
                //    throw new SportConflictException();
                foreach (var trainer in entity.Trainers!)
                {
                    context.Entry(trainer).State = EntityState.Detached;
                    if (context.Entry(trainer).State == EntityState.Detached)
                        context.Entry(trainer).State = EntityState.Unchanged;

                }

                EntityEntry<Sport> CreatedResult = await context.Set<Sport>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }
        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Sport? entity = await context.Set<Sport>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الرياضة غير موجودة");
            context.Set<Sport>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<Sport> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Sport? entity = await context.Set<Sport>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الرياضة غير موجودة");
            return entity!;
        }

        public async Task<IEnumerable<Sport>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Sport>? entities = await context.Set<Sport>().AsNoTracking().Include(x => x.Trainers).AsNoTracking().Where(x => x.IsActive).AsNoTracking().ToListAsync();
                return entities;
            }
        }

        public async Task<Sport> Update(Sport entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Sport existed_sport = await Get(entity.Id);
            if (existed_sport == null)
                throw new NotExistException("هذه الرياضة غير موجودة");
            foreach (var trainer in entity.Trainers!)
            {
                context.Entry(trainer).State = EntityState.Detached;
                if (context.Entry(trainer).State == EntityState.Detached)
                    context.Entry(trainer).State = EntityState.Modified;
            }
            context.Entry(entity).State = EntityState.Detached;
            if (context.Entry(entity).State == EntityState.Detached)
                context.Entry(entity).State = EntityState.Modified;
            context.Set<Sport>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
