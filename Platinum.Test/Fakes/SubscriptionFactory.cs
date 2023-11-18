using Bogus;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.Sport;
using PlatinumGym.Core.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Test.Fakes
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
                  subscription.Player = player;
                  subscription.Trainer = trainer;
                  subscription.Price = sport.Price;
                  subscription.PaidValue = 0;
                  subscription.PriceAfterOffer = sport.Price;
                  subscription.RollDate = DateTime.Now;
                  subscription.RestValue = 0;
                  subscription.PrivatePrice = Convert.ToDouble(fake.Commerce.Price());
                  subscription.OfferValue = 0;
                  subscription.OfferDes = fake.Lorem.Paragraph();
                  subscription.LastPaid = DateTime.Now;
                  subscription.EndDate = subscription.RollDate.AddDays(sport.DaysCount);
                  subscription.IsPaid = false;

              });
            return subscriptionFaker;
        }

       
    }
}
