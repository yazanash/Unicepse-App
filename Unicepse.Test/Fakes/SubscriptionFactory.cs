using Bogus;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Test.Fakes
{
    public class SubscriptionFactory
    {
        public Subscription FakeSubscription(Sport sport,Player player , Employee trainer)
        {
            var subscriptionFaker = new Faker<Subscription>()
              .StrictMode(false)
              .Rules((fake, subscription) =>
              {
                  subscription.Sport = sport;
                  subscription.DaysCount = sport.DaysCount;
                  subscription.Player = player;
                  subscription.Trainer = trainer;
                  subscription.Price = sport.Price;
                  subscription.PaidValue = 0;
                  subscription.PriceAfterOffer = sport.Price;
                  subscription.RollDate = DateTime.Now.AddDays(-20);
                  subscription.RestValue = 0;
                  subscription.PrivatePrice =0;
                  subscription.OfferValue = 0;
                  subscription.OfferDes = "";
                  subscription.LastPaid = subscription.RollDate;
                  subscription.EndDate = subscription.RollDate.AddDays(sport.DaysCount);
                  subscription.IsPaid = false;

              });
            return subscriptionFaker;
        }

       
    }
}
