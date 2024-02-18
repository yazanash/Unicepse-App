using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.TrainingProgram;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.Services
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
            PlayerRoutine? entity = await context.Set<PlayerRoutine>().FirstOrDefaultAsync((e) => e.RoutineNo== playerRoutine.RoutineNo);
            return entity!;
        }
        public async Task<PlayerRoutine> Create(PlayerRoutine entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<PlayerRoutine> CreatedResult = await context.Set<PlayerRoutine>().AddAsync(entity);
                PlayerRoutine existed_routine = await CheckIfRoutineExist(entity);
                if (existed_routine != null)
                    throw new ConflictException();
                if(entity.Player!=null)
                context.Attach(entity.Player!);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine? entity = await context.Set<PlayerRoutine>().Include(x=>x.RoutineSchedule).FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();

            context.Set<RoutineItems>().RemoveRange(entity!.RoutineSchedule);
            context.Set<PlayerRoutine>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PlayerRoutine> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine? entity = await context.Set<PlayerRoutine>().Include(x=>x.RoutineSchedule).ThenInclude(x=>x.Exercise).FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }

        public async Task<IEnumerable<PlayerRoutine>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerRoutine>? entities = await context.Set<PlayerRoutine>().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercise).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<PlayerRoutine>> GetAll(Player player)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerRoutine>? entities = await context.Set<PlayerRoutine>().Where(x=>x.Player!.Id==player.Id).Include(x=>x.RoutineSchedule).ThenInclude(x => x.Exercise).ToListAsync();
                return entities;
            }
        }
        public async Task<PlayerRoutine> Update(PlayerRoutine entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerRoutine existed_employee = await Get(entity.Id);
            if (existed_employee == null)
                throw new NotExistException();
            context.Set<PlayerRoutine>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
