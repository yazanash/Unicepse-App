using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Subscription;
namespace Unicepse.Core.Models.Payment
{
    public class PlayerPayment : DomainObject
    {
        public virtual Player.Player? Player { get; set; }
        public double PaymentValue { get; set; }
        public virtual Employee.Employee? Recipient { get; set; }
        public string? Des { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int CoverDays { get; set; }
        public Subscription.Subscription? Subscription { get; set; }
    }
}
