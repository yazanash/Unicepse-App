using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.PlayerQueries
{
    public class PlayerDataService : IDataService<Player>, IPublicIdService<Player>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PlayerDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Player> Create(Player entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Player existedPlayer = await CheckIfExistByName(entity.FullName!);
            if (existedPlayer != null)
                throw new PlayerConflictException(existedPlayer, entity, "هذا اللاعب موجود بالفعل");
            EntityEntry<Player> CreatedResult = await context.Set<Player>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }
        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new PlayerNotExistException();
            context.Set<Player>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<Player?> GetByUID(string uid)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.UID == uid);
            return entity!;
        }
        public async Task<Player> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new PlayerNotExistException("هذا اللاعب غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<Player>> GetAll()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Player>? entities = await context.Set<Player>().Select(p => new Player
            {
                Id = p.Id,
                SyncId =p.SyncId,
                FullName = p.FullName,
                Phone = p.Phone,
                BirthDate = p.BirthDate,
                GenderMale = p.GenderMale,
                MediclStatus = p.MediclStatus,
                SubscribeDate = p.SubscribeDate,
                SubscribeEndDate = p.Subscriptions.Count() > 0 ? p.Subscriptions.OrderByDescending(x => x.EndDate).FirstOrDefault()!.EndDate : p.SubscribeDate.AddDays(30),
                IsTakenContainer = p.IsTakenContainer,
                IsSubscribed = p.IsSubscribed,
                FingerprintData = p.FingerprintData,
                Balance = p.Payments.Sum(py => py.PaymentValue) - p.Subscriptions.Sum(s => s.PriceAfterOffer)
            }).Where(x => x.IsSubscribed == true).ToListAsync();
            return entities;

        }
        public async Task<Player> Update(Player entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Player existedPlayer = await Get(entity.Id);
            if (existedPlayer == null)
                throw new PlayerNotExistException("هذا اللاعب غير موجود");

            existedPlayer.FullName = entity.FullName;
            existedPlayer.MediclStatus = entity.MediclStatus;
            existedPlayer.SubscribeDate = entity.SubscribeDate;
            existedPlayer.SubscribeEndDate = entity.SubscribeEndDate;
            existedPlayer.IsTakenContainer = entity.IsTakenContainer;
            existedPlayer.IsSubscribed = entity.IsSubscribed;
            existedPlayer.UID = entity.UID;
            existedPlayer.Phone = entity.Phone;
            existedPlayer.BirthDate = entity.BirthDate;
            existedPlayer.GenderMale = entity.GenderMale;

            await context.Set<Subscription>()
            .Where(s => s.PlayerId == entity.Id)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(s => s.PlayerName, entity.FullName)
                .SetProperty(s => s.PlayerPhone, entity.Phone));
            context.Set<Player>().Update(existedPlayer);

            await context.SaveChangesAsync();
            return entity;

        }
        public async Task<Player> CheckIfExistByName(string name)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Player? entity = await context.Set<Player>().FirstOrDefaultAsync((e) => e.FullName == name);
            return entity!;
        }

    }
}
