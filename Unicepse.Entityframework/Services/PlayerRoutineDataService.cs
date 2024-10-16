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

        public PlayerRoutineDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
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
                throw new NotExistException();

            context.Set<RoutineItems>().RemoveRange(entity!.RoutineSchedule);
            context.Set<PlayerRoutine>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteRoutineItems(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<RoutineItems>? entity = await context.Set<RoutineItems>().Where(x => x.PlayerRoutine!.Id == id).ToListAsync();

            context.Set<RoutineItems>().RemoveRange(entity);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<PlayerRoutine> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine? entity = await context.Set<PlayerRoutine>().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercises).AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
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
        public async Task<IEnumerable<PlayerRoutine>> GetAllTemp()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerRoutine>? entities = await context.Set<PlayerRoutine>().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercises).AsNoTracking().Where(x => x.IsTemplate == true).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Exercises>> GetAllExercises()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Exercises>? entities = await context.Set<Exercises>().AsNoTracking().ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<PlayerRoutine>> GetAll(Player player)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerRoutine>? entities = await context.Set<PlayerRoutine>().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercises).Include(x => x.Player).Where(x => x.Player!.Id == player.Id).ToListAsync();
                return entities;
            }
        }
        public async Task<PlayerRoutine> Update(PlayerRoutine entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine existed_employee = await Get(entity.Id);
            if (existed_employee == null)
                throw new NotExistException();

            await DeleteRoutineItems(entity.Id);

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
        public async Task<PlayerRoutine> UpdateDataStatus(PlayerRoutine entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine existedPlayer = await Get(entity.Id);
            if (existedPlayer == null)
                throw new ConflictException("this routine is not existed");
            context.Entry(entity).Property(e => e.DataStatus).IsModified = true;
            await context.SaveChangesAsync();
            return entity;

        }
        public async Task<IEnumerable<PlayerRoutine>> GetByDataStatus(DataStatus status)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerRoutine>? entities = await context.Set<PlayerRoutine>().Include(x => x.Player).AsNoTracking().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercises).AsNoTracking().Where(x => x.DataStatus == status).ToListAsync();
                return entities;
            }
        }
    }
}
