using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;

namespace Uniceps.Test.Fakes
{
    public class SubscriptionFactory
    {
        public Subscription FakeSubscription(Sport sport, Player player, Employee trainer)
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
                  subscription.OfferValue = 0;
                  subscription.OfferDes = "";
                  subscription.LastPaid = subscription.RollDate;
                  subscription.EndDate = subscription.RollDate.AddDays(sport.DaysCount);
                  subscription.IsPaid = false;

              });
            return subscriptionFaker;
        }
        public Subscription FakeSubscriptionWithRandom(Sport sport, Employee trainer)
        {
            var subscriptionFaker = new Faker<Subscription>()
              .StrictMode(false)
              .Rules((fake, subscription) =>
              {
                  subscription.Id = fake.Random.Int(10000, 12222);
                  subscription.Sport = sport;
                  subscription.DaysCount = sport.DaysCount;
                  subscription.Player = new Player() { Id = fake.Random.Int(10000, 12222) };
                  subscription.Trainer = trainer;
                  subscription.Price = sport.Price;
                  subscription.PaidValue = 0;
                  subscription.PriceAfterOffer = sport.Price;
                  subscription.RollDate = DateTime.Now.AddDays(-20);
                  subscription.RestValue = 0;
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
