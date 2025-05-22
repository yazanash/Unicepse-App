using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models;
using Uniceps.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class PlayersAttendenceService
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PlayersAttendenceService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<DailyPlayerReport> LogInPlayer(DailyPlayerReport entity)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                entity.Player!.Subscriptions.Clear();
                entity.Player!.Payments.Clear();
                EntityEntry<DailyPlayerReport> CreatedResult = await context.Set<DailyPlayerReport>().AddAsync(entity);
                DailyPlayerReport? dailyPlayerReport = context.DailyPlayerReport!.Where(x =>
                x.Player!.Id == entity.Player!.Id &&
                x.Date.Month == entity.Date.Month &&
                x.Date.Year == entity.Date.Year &&
                x.Date.Day == entity.Date.Day &&
                x.IsLogged
                ).SingleOrDefault();
                if (dailyPlayerReport != null)
                {
                    throw new PlayerConflictException("هذا اللاعب تم تسجيل دخوله بالفعل ولم يسجل خروجه بعد");
                }
                entity.Player!.Subscriptions.Clear();
                entity.Player!.Payments.Clear();
                context.Entry(entity.Player!).State = EntityState.Unchanged;

                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<DailyPlayerReport> Update(DailyPlayerReport entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            DailyPlayerReport existedPlayer = await Get(entity.Id);
            if (existedPlayer == null)
                throw new NotExistException("هذا السجل غير موجود");

            context.Set<DailyPlayerReport>().Update(entity);
            await context.SaveChangesAsync();
            return entity;

        }
        public async Task<DailyPlayerReport> AddKey(DailyPlayerReport entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            DailyPlayerReport existedPlayer = await Get(entity.Id);
            if (existedPlayer == null)
                throw new NotExistException("هذا السجل غير موجود");
            DailyPlayerReport? dailyPlayerReport = context.DailyPlayerReport!.Where(x =>
               x.IsLogged
               && x.KeyNumber == entity.KeyNumber
               ).SingleOrDefault();
            if (dailyPlayerReport != null)
            {
                throw new PlayerConflictException("هذا المفتاح مع لاعب اخر لم يخرج بعد");
            }
            context.Set<DailyPlayerReport>().Update(entity);
            await context.SaveChangesAsync();
            return entity;

        }
        public async Task<bool> LogOutPlayer(DailyPlayerReport entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            context.Set<DailyPlayerReport>().Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<DailyPlayerReport> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            DailyPlayerReport? entity = await context.Set<DailyPlayerReport>().Include(x => x.Player).FirstOrDefaultAsync((e) => e.Id == id);
            return entity!;
        }
        public async Task<DailyPlayerReport?> Get(DailyPlayerReport entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            DailyPlayerReport? Getentity = await context.Set<DailyPlayerReport>().Include(x => x.Player).FirstOrDefaultAsync(x => x.Player!.Id == entity.Player!.Id &&
                                x.Date.Month == entity.Date.Month &&
                                x.Date.Year == entity.Date.Year &&
                                x.Date.Day == entity.Date.Day &&
                                x.IsLogged);
            return Getentity;
        }

        public async Task<IEnumerable<DailyPlayerReport>> GetLoggedPlayers(DateTime date)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<DailyPlayerReport>? entities = await context.Set<DailyPlayerReport>().Where(x => x.Date.Month == date.Month &&
            x.Date.Year == date.Year &&
            x.Date.Day == date.Day).Include(x => x.Player).AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<DailyPlayerReport>> GetPlayerLogging(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<DailyPlayerReport>? entities = await context.Set<DailyPlayerReport>().Where(
                x => x.Player!.Id == id
                ).Include(x => x.Player).ToListAsync();
            return entities;
        }

    }
}
