using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.PlayerQueries
{
    public class ArchivedPlayerService : IArchivedService<Player>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public ArchivedPlayerService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Player>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Player>? entities = await context.Set<Player>().Select(p => new Player
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    Phone = p.Phone,
                    BirthDate = p.BirthDate,
                    GenderMale = p.GenderMale,
                    Weight = p.Weight,
                    Hieght = p.Hieght,
                    SubscribeDate = p.SubscribeDate,
                    SubscribeEndDate = p.Subscriptions.Count() > 0 ? p.Subscriptions.OrderByDescending(x => x.EndDate).FirstOrDefault()!.EndDate : p.SubscribeDate.AddDays(30),
                    IsTakenContainer = p.IsTakenContainer,
                    IsSubscribed = p.IsSubscribed,
                    UID = p.UID,
                    Balance = p.Subscriptions.Sum(s => s.PriceAfterOffer) - p.Payments.Sum(py => py.PaymentValue)
                }).Where(x => x.IsSubscribed == false).ToListAsync();
                return entities;
            }
        }
    }
}
