using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Test.Fakes
{
    public class PaymentFactory
    {
        public PlayerPayment FakePayments(Subscription subscription)
        {
            var subscriptionFaker = new Faker<PlayerPayment>()
              .StrictMode(false)
              .Rules((fake, payment) =>
              {
                  payment.Subscription = subscription;
                  payment.PaymentValue = Convert.ToDouble(fake.Commerce.Price());
                  payment.PayDate = DateTime.Now;
                  payment.Des = fake.Lorem.Paragraph();
                  payment.Player = subscription.Player;
              });
            return subscriptionFaker;
        }
    }
}
