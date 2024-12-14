using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.TrainingProgram;
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
    public class PlayerRoutineDataService : IDataService<PlayerRoutine>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        private readonly IRoutineItemsDataService _routineItemsDataService;

        public PlayerRoutineDataService(PlatinumGymDbContextFactory contextFactory, IRoutineItemsDataService routineItemsDataService)
        {
            _contextFactory = contextFactory;
            _routineItemsDataService = routineItemsDataService;
        }
        public async Task<PlayerRoutine> CheckIfRoutineExist(PlayerRoutine playerRoutine)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine? entity = await context.Set<PlayerRoutine>().FirstOrDefaultAsync((e) => e.RoutineNo == playerRoutine.RoutineNo);
            return entity!;
        }
        public async Task<PlayerRoutine> Create(PlayerRoutine entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {

                foreach (var sc in entity.RoutineSchedule!)
                {
                    sc.Id = 0;
                    sc.Exercises = context.Exercises!.Find(sc.Exercises!.Id);
                    context.Entry(sc.Exercises!).State = EntityState.Detached;
                    if (context.Entry(sc.Exercises!).State == EntityState.Detached)
                        context.Entry(sc.Exercises!).State = EntityState.Unchanged;

                }
                context.Entry(entity.Player!).State = EntityState.Detached;
                if (context.Entry(entity.Player!).State == EntityState.Detached)
                    context.Entry(entity.Player!).State = EntityState.Unchanged;
                EntityEntry<PlayerRoutine> CreatedResult = await context.Set<PlayerRoutine>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine? entity = await context.Set<PlayerRoutine>().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercises).FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");

            context.Set<RoutineItems>().RemoveRange(entity!.RoutineSchedule);
            context.Set<PlayerRoutine>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<PlayerRoutine> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine? entity = await context.Set<PlayerRoutine>().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercises).AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<PlayerRoutine>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerRoutine>? entities = await context.Set<PlayerRoutine>().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercises).AsNoTracking().ToListAsync();
                return entities;
            }
        }
        public async Task<PlayerRoutine> Update(PlayerRoutine entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine existed_employee = await Get(entity.Id);
            if (existed_employee == null)
                throw new NotExistException("هذا السجل غير موجود");

            await _routineItemsDataService.DeleteRoutineItems(entity.Id);

            foreach (var sc in entity.RoutineSchedule!)
            {
                sc.Id = 0;
                sc.PlayerRoutine = entity;
                sc.Exercises = context.Exercises!.Find(sc.Exercises!.Id);
                context.Entry(sc).State = EntityState.Detached;
                if (context.Entry(sc).State == EntityState.Detached)
                    context.Entry(sc).State = EntityState.Added;

            }
            context.Set<PlayerRoutine>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
       
    }
}
