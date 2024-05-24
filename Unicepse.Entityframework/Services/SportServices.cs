using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Entityframework.Services
{
    public class SportServices : IDataService<Sport>
    {

        private readonly PlatinumGymDbContextFactory _contextFactory;

        public SportServices(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Sport?> CheckIfExistByName(string name)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
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
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                //Sport? existed_sport = await CheckIfExistByName(entity.Name!);
                //if (existed_sport != null)
                //    throw new SportConflictException();
                foreach (var trainer in entity.Trainers!)
                {
                    context.Entry(trainer).State = EntityState.Detached;
                    context.Attach(trainer);

                }
                EntityEntry<Sport> CreatedResult = await context.Set<Sport>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }
        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Sport? entity = await context.Set<Sport>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الرياضة غير موجودة");
            context.Set<Sport>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteConnectedTrainers(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Sport? entity = await context.Set<Sport>().AsNoTracking().Include(x=>x.Trainers).AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الرياضة غير موجودة");
            entity.Trainers!.Clear();
            context.Set<Sport>().Update(entity!);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<Sport> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Sport? entity = await context.Set<Sport>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الرياضة غير موجودة");
            return entity!;
        }

        public async Task<IEnumerable<Sport>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Sport>? entities = await context.Set<Sport>().AsNoTracking().Include(x => x.Trainers).AsNoTracking().Where(x=>x.IsActive).ToListAsync();
                return entities;
            }
        }

        public async Task<Sport> Update(Sport entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Sport existed_sport = await Get(entity.Id);
            if (existed_sport == null)
                throw new NotExistException("هذه الرياضة غير موجودة");
            foreach (var trainer in entity.Trainers!)
            {
                context.Entry(trainer).State = EntityState.Detached;
                context.Attach(trainer);
            }
            context.Set<Sport>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
