using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.TransactionsReportServices
{
    public class ActiveSubsecriptionService : IActiveTransactionService<Subscription>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public ActiveSubsecriptionService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Subscription>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().Where(x => x.EndDate >= DateTime.Now).Include(x => x.Trainer).AsNoTracking()
                    .Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Sport).AsNoTracking().Select(p => new Subscription
                    {
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
                        IsPaid = p.Payments!.Any() && p.Payments!.Sum(x => x.PaymentValue) >= p.PriceAfterOffer,
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
