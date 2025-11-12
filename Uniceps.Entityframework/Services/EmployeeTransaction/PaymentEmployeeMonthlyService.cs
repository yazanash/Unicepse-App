using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Entityframework.Services.EmployeeTransaction
{
    public class PaymentEmployeeMonthlyService : IEmployeeMonthlyTransaction<PlayerPayment>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public PaymentEmployeeMonthlyService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<PlayerPayment>> GetAll(Employee trainer, DateTime date)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).AsNoTracking()
                    .Include(x => x.Subscription!.Player).AsNoTracking()
                    .Include(x => x.Subscription!.Sport).AsNoTracking()
                    .Include(x => x.Subscription!.Trainer).AsNoTracking()
                    .Where(x => x.Subscription!.Trainer!.Id == trainer.Id &&

                   (x.PayDate.Month == date.Month && x.PayDate.Year == date.Year ||
                    x.To.Month == date.Month && x.To.Year == date.Year) && x.DataStatus != DataStatus.ToDelete

                    ).AsNoTracking()
                    .Select(p => new PlayerPayment
                    {
                        Subscription = new Subscription
                        {
                            Id = p.Subscription!.Id,
                            Sport = p.Subscription!.Sport,
                            LastCheck = p.Subscription!.LastCheck,
                            TrainerId = p.Subscription!.TrainerId,
                            Trainer = p.Subscription!.Trainer,
                            PrevTrainer_Id = p.Subscription!.PrevTrainer_Id,
                            Player = p.Subscription!.Player,
                            RollDate = p.Subscription!.RollDate,
                            Price = p.Subscription!.Price,
                            OfferValue = p.Subscription!.OfferValue,
                            OfferDes = p.Subscription!.OfferDes,
                            PriceAfterOffer = p.Subscription!.PriceAfterOffer,
                            MonthCount = p.Subscription!.MonthCount,
                            DaysCount = p.Subscription!.DaysCount,
                            IsStopped = p.Subscription!.IsStopped,
                            IsPaid = p.Subscription!.Payments!.Any() && p.Subscription!.Payments!.Sum(x => x.PaymentValue) >= p.Subscription!.PriceAfterOffer || p.Subscription!.PriceAfterOffer == 0,
                            PaidValue = p.Subscription!.Payments!.Sum(x => x.PaymentValue),
                            RestValue = p.Subscription!.Payments!.Sum(x => x.PaymentValue) - p.Subscription!.PriceAfterOffer,
                            EndDate = p.Subscription!.EndDate,
                            LastPaid = p.Subscription!.LastPaid,
                        },
                        Player = p.Player,
                        PaymentValue = p.PaymentValue,
                        Des = p.Des,
                        PayDate = p.PayDate,
                        From = p.From,
                        To = p.To,
                        CoverDays = p.CoverDays,
                    })
                    .ToListAsync();
                return entities;
            }
        }
    }
}
