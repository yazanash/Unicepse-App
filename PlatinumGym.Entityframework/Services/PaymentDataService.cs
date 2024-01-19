using PlatinumGym.Core.Services;
using PlatinumGym.Core.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Entityframework.DbContexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using PlatinumGym.Core.Models.Player;

namespace PlatinumGym.Entityframework.Services
{
    public class PaymentDataService : IDataService<PlayerPayment>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public PaymentDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PlayerPayment> Create(PlayerPayment entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            context.Attach(entity.Subscription!);
            context.Attach(entity.Player!);
            EntityEntry<PlayerPayment> CreatedResult = await context.Set<PlayerPayment>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment? entity = await context.Set<PlayerPayment>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            context.Set<PlayerPayment>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PlayerPayment> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment? entity = await context.Set<PlayerPayment>().Include(x=>x.Player)
                .Include(x=>x.Subscription).FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }

        public async Task<IEnumerable<PlayerPayment>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player)
                    .Include(x=>x.Subscription)
                    .ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<PlayerPayment>> GetPlayerPayments(Player player)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player)
                    .Include(x => x.Subscription).Where(x => x.Player!.Id == player.Id)
                    .ToListAsync();
                return entities;
            }
        }
        public async Task<PlayerPayment> Update(PlayerPayment entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment existed_payment = await Get(entity.Id);
            if (existed_payment == null)
                throw new NotExistException();
            context.Set<PlayerPayment>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
