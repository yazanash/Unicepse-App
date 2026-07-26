using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.MessengerSystem.Events
{
    public record EntityUpdated<T>(T Entity) where T : class;
    public record PaymentCreated(int SubscriptionId, double AmountPaid, DateTime DatePaid);

    public record PaymentDeleted(int SubscriptionId, double AmountPaid);
}
