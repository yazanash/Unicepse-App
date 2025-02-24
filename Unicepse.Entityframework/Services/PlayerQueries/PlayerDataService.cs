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
using Unicepse.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

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
                throw new PlayerNotExistException("هذا اللاعب غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<Player>> GetAll()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<Player>? entities = await context.Set<Player>().Select(p => new Player
            {
                Id = p.Id,
                FullName  =p.FullName,
                Phone = p.Phone,
                BirthDate = p.BirthDate,
                GenderMale = p.GenderMale,
                Weight = p.Weight,
                Hieght = p.Hieght,
                SubscribeDate = p.SubscribeDate,
                SubscribeEndDate =p.Subscriptions.Count()>0? p.Subscriptions.OrderByDescending(x => x.EndDate).FirstOrDefault()!.EndDate: p.SubscribeDate.AddDays(30),
                IsTakenContainer = p.IsTakenContainer,
                IsSubscribed = p.IsSubscribed,
                UID = p.UID,
                Balance = p.Subscriptions.Sum(s => s.PriceAfterOffer) - p.Payments.Sum(py => py.PaymentValue)
            }).Where(x => x.IsSubscribed == true).ToListAsync();
            return entities;

        }
        public async Task<Player> Update(Player entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Player existedPlayer = await Get(entity.Id);
            if (existedPlayer == null)
                throw new PlayerNotExistException("هذا اللاعب غير موجود");
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
