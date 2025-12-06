using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.Payment
{
    public class PayReferance : DomainObject
    {

        public Player.Player? Player { get; set; }
        public Subscription.Subscription? PlayerTraining { get; set; }
        public double Value { get; set; }
        public DateTime RefDate { get; set; }
    }
}
