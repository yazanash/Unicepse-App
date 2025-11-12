using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.GetPlayerTransaction
{
    public class GetSubscriptionsService : IGetPlayerTransactionService<Subscription>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public GetSubscriptionsService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Subscription>> GetAll(Player player)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Subscription>? entities = await context.Set<Subscription>().AsNoTracking().Where(x => x.Player!.Id == player.Id).Include(x => x.Trainer)
                    .Include(x => x.Player).AsNoTracking().Include(x => x.Sport!.Trainers).AsNoTracking()
                    .Include(x => x.Sport).AsNoTracking().AsNoTracking().Select(p => new Subscription
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
