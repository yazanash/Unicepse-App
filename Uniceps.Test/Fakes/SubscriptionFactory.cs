using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;

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
                  subscription.SportName = sport.Name;
                  subscription.SportId = sport.Id;
                  subscription.DaysCount = sport.DaysCount;
                  subscription.PlayerName = player.FullName;
                  subscription.SportId= player.Id;
                  subscription.TrainerId = trainer.Id;
                  subscription.TrainerName = trainer.FullName;
                  subscription.Price = sport.Price;
                  subscription.PriceAfterOffer = sport.Price;
                  subscription.RollDate = DateTime.Now.AddDays(-20);
                  subscription.OfferValue = 0;
                  subscription.OfferDes = "";
                  subscription.EndDate = subscription.RollDate.AddDays(sport.DaysCount);

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
                  subscription.SportName = sport.Name;
                  subscription.SportId = sport.Id;
                  subscription.DaysCount = sport.DaysCount;
                  subscription.TrainerId = trainer.Id;
                  subscription.TrainerName = trainer.FullName;
                  subscription.Price = sport.Price;
                  subscription.PriceAfterOffer = sport.Price;
                  subscription.RollDate = DateTime.Now.AddDays(-20);
                  subscription.OfferValue = 0;
                  subscription.OfferDes = "";
                  subscription.EndDate = subscription.RollDate.AddDays(sport.DaysCount);

              });
            return subscriptionFaker;
        }

    }
}
