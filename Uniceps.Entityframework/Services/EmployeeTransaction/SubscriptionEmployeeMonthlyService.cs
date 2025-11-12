using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.EmployeeTransaction
{
    public class SubscriptionEmployeeMonthlyService : IEmployeeMonthlyTransaction<Subscription>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public SubscriptionEmployeeMonthlyService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Subscription>> GetAll(Employee trainer, DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.Trainer!.Id == trainer.Id
                && (x.RollDate.Month == date.Month && x.RollDate.Year == date.Year
                || x.EndDate.Month == date.Month && x.EndDate.Year == date.Year)).Include(x => x.Trainer).AsNoTracking()
                    .Include(x => x.Player).AsNoTracking().Include(x => x.Sport!.Trainers).AsNoTracking()
                    .Include(x => x.Sport).Select(p => new Subscription
                    {
                        Id = p.Id,
                        Sport = p.Sport,
                        LastCheck = p.LastCheck,
                        TrainerId = p.TrainerId,
                        Trainer = p.Trainer,
                        PrevTrainer_Id = p.PrevTrainer_Id,
                        Player = p.Player,
                        RollDate = p.RollDate,
                        Price = p.Price,
                        OfferValue = p.OfferValue,
                        OfferDes = p.OfferDes,
                        PriceAfterOffer = p.PriceAfterOffer,
                        MonthCount = p.MonthCount,
                        DaysCount = p.DaysCount,
                        IsStopped = p.IsStopped,
                        IsPaid = p.Payments!.Any() && p.Payments!.Sum(x => x.PaymentValue) >= p.PriceAfterOffer || p.PriceAfterOffer == 0,
                        PaidValue = p.Payments!.Sum(x => x.PaymentValue),
                        RestValue = p.Payments!.Sum(x => x.PaymentValue) - p.PriceAfterOffer,
                        EndDate = p.EndDate,
                        LastPaid = p.LastPaid,

                    }).ToListAsync();
                return entities;
            }
        }
    }
}
