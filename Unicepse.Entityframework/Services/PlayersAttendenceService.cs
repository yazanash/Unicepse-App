using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models;
using Unicepse.Core.Models.DailyActivity;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Entityframework.Services
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
                DailyPlayerReport? dailyPlayerReport = context.DailyPlayerReport!.Where(x =>
                x.Player!.Id == entity.Player!.Id &&
                x.Date.Month == entity.Date.Month &&
                x.Date.Year == entity.Date.Year &&
                x.Date.Day == entity.Date.Day&&
                x.IsLogged
                ).SingleOrDefault();
                if (dailyPlayerReport != null)
                {
                    throw new PlayerConflictException("هذا اللاعب تم تسجيل دخوله بالفعل ولم يسجل خروجه بعد");
                }
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

            IEnumerable<DailyPlayerReport>? entities = await context.Set<DailyPlayerReport>().Where(x=>x.Date.Month==DateTime.Now.Month&&
            x.Date.Year == DateTime.Now.Year&&
            x.Date.Day == DateTime.Now.Day).Include(x=>x.Player).AsNoTracking().ToListAsync();
            return entities;
        }
        public async Task<IEnumerable<DailyPlayerReport>> GetPlayerLogging(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<DailyPlayerReport>? entities = await context.Set<DailyPlayerReport>().Where(
                x => x.Player!.Id==id
                ).Include(x=>x.Player).ToListAsync();
            return entities;
        }
      
    }
}
