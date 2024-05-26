using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Entityframework.Services.PlayerQueries
{
    public class PlayerDataService: IDataService<Player>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public PlayerDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }


        public async Task<Player> Create(Player entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Player existedPlayer = await CheckIfExistByName(entity.FullName!);
            if(existedPlayer !=null)
                throw new PlayerConflictException(existedPlayer,entity,"هذا اللاعب موجود بالفعل");
            EntityEntry<Player> CreatedResult = await context.Set<Player>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new PlayerNotExistException();
            context.Set<Player>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Player> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new PlayerNotExistException();
            return entity!;
        }

        public async Task<IEnumerable<Player>> GetAll()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Player>? entities = await context.Set<Player>().ToListAsync();
            return entities;

        }
        public async Task<IEnumerable<Player>> GetByStatus(bool status)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Player>? entities = await context.Set<Player>().Where(x => x.IsSubscribed == status).ToListAsync();
                return entities;
            }
        }
        public async Task<Player> Update(Player entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Player existedPlayer = await Get(entity.Id);
            if (existedPlayer == null)
                throw new PlayerConflictException(existedPlayer!, entity, "this player is existed");
            context.Set<Player>().Update(entity);
            await context.SaveChangesAsync();
            return entity;

        }
        public async Task<Player> CheckIfExistByName(string name)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.FullName == name);
            return entity!;
        }
    }
}
