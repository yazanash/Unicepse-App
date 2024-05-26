using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models;
using Unicepse.Core.Models.Subscription;

namespace Unicepse.Core.Models.Payment
{
    public class PayReferance : DomainObject
    {
       
        public Player.Player? Player { get; set; }
        public Subscription.Subscription? PlayerTraining { get; set; }
        public double Value { get; set; }
        public DateTime RefDate { get; set; }
    }
}
