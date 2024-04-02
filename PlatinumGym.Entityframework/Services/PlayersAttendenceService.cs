using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Models;
using PlatinumGym.Core.Models.DailyActivity;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.Services
{
    public class PlayersAttendenceService 
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public PlayersAttendenceService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<DailyPlayerReport> LogInPlayer(DailyPlayerReport entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {

                EntityEntry<DailyPlayerReport> CreatedResult = await context.Set<DailyPlayerReport>().AddAsync(entity);
                context.Attach(entity.Player!);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> LogOutPlayer(DailyPlayerReport entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            context.Set<DailyPlayerReport>().Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<DailyPlayerReport> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            DailyPlayerReport? entity = await context.Set<DailyPlayerReport>().FirstOrDefaultAsync((e) => e.Id == id);
            return entity!;
        }

        public async Task<IEnumerable<DailyPlayerReport>> GetLoggedPlayers()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<DailyPlayerReport>? entities = await context.Set<DailyPlayerReport>().Where(x=>x.IsLogged).Include(x=>x.Player).AsNoTracking().ToListAsync();
            return entities;
        }
        public async Task<IEnumerable<DailyPlayerReport>> GetPlayerLogging(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<DailyPlayerReport>? entities = await context.Set<DailyPlayerReport>().Where(
                x => x.Player!.Id==id
                ).ToListAsync();
            return entities;
        }
      
    }
}
