﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGymPro.DbContexts;
using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Services.PlayerQueries
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
            EntityEntry<Player> CreatedResult = await context.Set<Player>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<Player>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Player> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.Id == id);
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
        public async Task<IEnumerable<Player>> GetByGender(bool GenderMale)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Player>? entities = await context.Set<Player>().Where(x => x.GenderMale == GenderMale).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Player>> GetBySubscribeEnd()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Player>? entities = await context.Set<Player>().Where(x => x.SubscribeEndDate < DateTime.Now).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Player>> GetByDebt()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Player>? entities = await context.Set<Player>().Where(x => x.Balance < 0).ToListAsync();
                return entities;
            }
        }
        public async Task<Player> Update(Player entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            context.Set<Player>().Update(entity);
            await context.SaveChangesAsync();
            return entity;

        }
    }
}