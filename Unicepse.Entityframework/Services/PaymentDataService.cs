using Unicepse.Core.Services;
using Unicepse.Core.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Entityframework.DbContexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Common;

namespace Unicepse.Entityframework.Services
{
    public class PaymentDataService : IPaymentDataService
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
            //context.Attach(entity.Subscription!.Player!);
            //context.tity.Player!);
            double dayPrice = entity.Subscription!.PriceAfterOffer / entity.Subscription!.DaysCount;
            entity.CoverDays = (int)(entity.PaymentValue / dayPrice);
            entity.To = entity.From.AddDays(entity.CoverDays);
            EntityEntry<PlayerPayment> CreatedResult = await context.Set<PlayerPayment>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment? entity = await context.Set<PlayerPayment>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<PlayerPayment>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PlayerPayment> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment? entity = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                .Include(x => x.Subscription).AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<PlayerPayment>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).AsNoTracking()
                    .ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<PlayerPayment>> GetAll(DateTime dateFrom, DateTime dateTo)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).Include(x => x.Subscription!.Sport).Include(x => x.Subscription!.Trainer).AsNoTracking().Where(x => x.PayDate >= dateFrom && x.PayDate <= dateTo && x.DataStatus != DataStatus.ToDelete)
                    .ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<PlayerPayment>> GetAll(DateTime dateTo)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).Include(x => x.Subscription!.Sport).Include(x => x.Subscription!.Trainer).AsNoTracking()
                    .Where(x => x.PayDate.Month == dateTo.Month && x.PayDate.Year == dateTo.Year && x.PayDate.Day == dateTo.Day && x.DataStatus != DataStatus.ToDelete)
                    .ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<PlayerPayment>> GetPlayerPayments(Player player)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).AsNoTracking().AsNoTracking().Include(x => x.Subscription!.Sport).AsNoTracking().Include(x => x.Subscription!.Trainer).AsNoTracking().Where(x => x.Player!.Id == player.Id && x.DataStatus != DataStatus.ToDelete).AsNoTracking()
                   .AsNoTracking().ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<PlayerPayment>> GetTrainerPayments(Employee trainer, DateTime date)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).AsNoTracking()
                    .Include(x => x.Subscription!.Player).AsNoTracking()
                    .Include(x => x.Subscription!.Sport).AsNoTracking()
                    .Include(x => x.Subscription!.Trainer).AsNoTracking()
                    .Where(x => x.Subscription!.Trainer!.Id == trainer.Id &&
                    (
                   (x.PayDate.Month == date.Month && x.PayDate.Year == date.Year ||
                    x.To.Month == date.Month && x.To.Year == date.Year) && x.DataStatus != DataStatus.ToDelete
                    )
                    ).AsNoTracking()
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<Employee> GetPreviousTrainer(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Employee? entity = await context.Set<Employee>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }
        public async Task<IEnumerable<PlayerPayment>> GetByDataStatus(DataStatus status)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Where(x => x.DataStatus == status).Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).AsNoTracking().ToListAsync();
                return entities;
            }
        }
        public async Task<PlayerPayment> UpdateDataStatus(PlayerPayment entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment? dataToSync =await context.PlayerPayments!.FindAsync(entity.Id);
            if (dataToSync == null)
                throw new NotExistException("هذا السجل غير موجود");

            dataToSync.DataStatus = entity.DataStatus;
            context.Entry(dataToSync).Property(e => e.DataStatus).IsModified = true;
            await context.SaveChangesAsync();
            return entity;

        }
        public async Task<PlayerPayment> Update(PlayerPayment entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment existed_payment = await Get(entity.Id);
            if (existed_payment == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Entry(entity).State = EntityState.Detached;
            if (context.Entry(entity).State == EntityState.Detached)
            {
                // Attach the entity
                context.Entry(entity).State = EntityState.Modified;
            }
            //context.Attach(entity);
            context.Set<PlayerPayment>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
