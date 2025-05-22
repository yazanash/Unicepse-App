using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class PaymentDataService : IDataService<PlayerPayment>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PaymentDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PlayerPayment> Create(PlayerPayment entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
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
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment? entity = await context.Set<PlayerPayment>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            context.Set<PlayerPayment>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PlayerPayment> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment? entity = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                .Include(x => x.Subscription).AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<PlayerPayment>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).AsNoTracking()
                    .ToListAsync();
                return entities;
            }
        }
        //public async Task<Employee> GetPreviousTrainer(int id)
        //{
        //    using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
        //    Employee? entity = await context.Set<Employee>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
        //    if (entity == null)
        //        throw new NotExistException("هذا السجل غير موجود");
        //    return entity!;
        //}


        public async Task<PlayerPayment> Update(PlayerPayment entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
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
