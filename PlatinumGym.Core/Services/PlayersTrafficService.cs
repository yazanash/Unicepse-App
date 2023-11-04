using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGymPro.DbContexts;
using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Services
{
    public class PlayersTrafficService : IDataService<DailyPlayerReport>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public PlayersTrafficService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<DailyPlayerReport> Create(DailyPlayerReport entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {

                EntityEntry<DailyPlayerReport> CreatedResult = await context.Set<DailyPlayerReport>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            DailyPlayerReport? entity = await context.Set<DailyPlayerReport>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<DailyPlayerReport>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<DailyPlayerReport> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            DailyPlayerReport? entity = await context.Set<DailyPlayerReport>().FirstOrDefaultAsync((e) => e.Id == id);
            return entity!;
        }

        public async Task<IEnumerable<DailyPlayerReport>> GetAll()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<DailyPlayerReport>? entities = await context.Set<DailyPlayerReport>().ToListAsync();
            return entities;
        }
        public async Task<IEnumerable<DailyPlayerReport>> GetActivatedPlayers()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<DailyPlayerReport>? entities = await context.Set<DailyPlayerReport>().Where(
                x => x.Date == DateTime.Now && x.logoutTime ==x.loginTime
                ).ToListAsync();
            return entities;
        }
        public async Task<DailyPlayerReport> Update(DailyPlayerReport entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            context.Set<DailyPlayerReport>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
