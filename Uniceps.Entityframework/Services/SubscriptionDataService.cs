using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Subscription;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class SubscriptionDataService : IDataService<Subscription>,
        ISubscriptionRenewService,
        ISportMonthlyTransactions<Subscription>,
        IGetPlayerTransactionService<Subscription>,
        IEmployeeMonthlyTransaction<Subscription>,
        IEmployeeTransaction<Subscription>
    {

        private readonly UnicepsDbContextFactory _contextFactory;

        public SubscriptionDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Subscription> CheckIfSubscriptionExist(Subscription subscription)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Subscription? entity = await context.Set<Subscription>().AsNoTracking().FirstOrDefaultAsync((e) => e.PlayerId == subscription.PlayerId &&
            e.SportId == subscription.SportId && e.EndDate >= subscription.RollDate);
            return entity!;
        }
        public async Task<Subscription> Create(Subscription entity)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                Subscription? existed_subscription = await context.Set<Subscription>().AsNoTracking().FirstOrDefaultAsync((e) => e.PlayerId == entity.PlayerId &&
            e.SportId == entity.SportId && e.EndDate >= entity.RollDate);
                if (existed_subscription != null)
                    throw new ConflictException("هذا اللاعب مسجل في هذه الرياضة مسبقا ");

                if (string.IsNullOrEmpty(entity.Code))
                {
                    bool isUnique = false;
                    string newCode = "";
                    int attempt = 0;
                    while (!isUnique)
                    {
                        newCode = entity.GenerateSubscriptionCode(attempt);

                        // نتحقق إذا كان الكود موجوداً في جدول الاشتراكات
                        bool codeExists = await context.Set<Subscription>()
                                                      .AnyAsync(x => x.Code == newCode);

                        if (!codeExists)
                            isUnique = true;
                        else
                        {
                            attempt++;
                        }

                        if (attempt > 1000) throw new Exception("تعذر توليد كود فريد");
                    }

                    entity.Code = newCode;
                }

                EntityEntry<Subscription> CreatedResult = await context.Set<Subscription>().AddAsync(entity);
                await context.SaveChangesAsync();

                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Subscription? entity = await context.Set<Subscription>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            List<PlayerPayment> pays = await context.Set<PlayerPayment>().Where(x => x.SubscriptionId == id).ToListAsync();
            context.Set<PlayerPayment>().RemoveRange(pays!);
            context.Set<Subscription>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Subscription> Get(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Subscription? entity = await context.Set<Subscription>()
                .FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا السجل غير موجود");
            return entity!;
        }
        public async Task<IEnumerable<Subscription>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().Where(x => x.RollDate<=DateTime.Now && x.EndDate >= DateTime.Now).Include(x=>x.Payments).ToListAsync();
                foreach(var entity in entities)
                {
                    entity.GetTotal();
                }
                return entities;
            }
        }
        public async Task<IEnumerable<Subscription>> GetAll(int daysPastEnd = 0)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                DateTime thresholdDate = DateTime.Now.AddDays(-daysPastEnd);

                IEnumerable<Subscription> entities = await context.Set<Subscription>()
                    .Where(x => x.RollDate <= DateTime.Now && x.EndDate >= thresholdDate)
                    .Include(x => x.Payments)
                    .ToListAsync();
                foreach (var entity in entities)
                {
                    entity.GetTotal();
                }
                return entities;
            }
        }
        public async Task<IEnumerable<Subscription>> GetAll(Sport sport, DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.SportId == sport.Id
                && (x.RollDate.Month == date.Month && x.RollDate.Year == date.Year
                || x.EndDate.Month == date.Month && x.EndDate.Year == date.Year)).Include(x => x.Payments).ToListAsync();
                foreach (var entity in entities)
                {
                    entity.GetTotal();
                }
                return entities;
            }
        }
        public async Task<IEnumerable<Subscription>> GetAllByPlayer(int playerId)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().Include(x => x.Payments).AsNoTracking().Where(x => x.PlayerId == playerId).ToListAsync();
                foreach (var entity in entities)
                {
                    entity.GetTotal();
                }
                return entities;
            }
        }
        public async Task<IEnumerable<Subscription>> GetAll(int trainerId, DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                var monthStart = new DateTime(date.Year, date.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.TrainerId == trainerId
                && x.RollDate <= monthEnd   // يبدأ قبل نهاية الشهر
                && x.EndDate >= monthStart).Include(x => x.Payments).ToListAsync();
                foreach (var entity in entities)
                {
                    entity.GetTotal();
                }
                return entities;
            }
        }
        public async Task<IEnumerable<Subscription>> GetAll(Employee trainer)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.TrainerId == trainer.Id).Include(x => x.Payments)
                   .ToListAsync();
                foreach (var entity in entities)
                {
                    entity.GetTotal();
                }
                return entities;
            }
        }
        public async Task<Subscription> Update(Subscription entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            Subscription? existed_subscription = await context.Set<Subscription>()
                .FirstOrDefaultAsync((e) => e.Id == entity.Id);
            if (existed_subscription == null)
                throw new NotExistException("هذا السجل غير موجود");
            existed_subscription.MergeWith(entity);
            context.Set<Subscription>().Update(entity);
            await context.SaveChangesAsync();
            Subscription? returnEntity = await context.Set<Subscription>().Include(x=>x.Payments)
                .FirstOrDefaultAsync((e) => e.Id == entity.Id);
            if (returnEntity != null)
            {
                returnEntity.GetTotal();
                return returnEntity;
            }
            entity.GetTotal();
            return entity;
        }
        public async Task<bool> MarkAsRenewed(int entityId)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            Subscription? existed_subscription = await context.Set<Subscription>()
                .FirstOrDefaultAsync((e) => e.Id == entityId);
            if (existed_subscription == null)
                throw new NotExistException("هذا السجل غير موجود");

            existed_subscription.IsRenewed = true;
            context.Set<Subscription>().Update(existed_subscription);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
