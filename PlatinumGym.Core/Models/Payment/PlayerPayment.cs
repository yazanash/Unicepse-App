using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.Subscription;
namespace PlatinumGym.Core.Models.Payment
{
    public class PlayerPayment : DomainObject
    {
        public virtual Player.Player? Player { get; set; }
        public double PaymentValue { get; set; }
        public virtual Employee.Employee? Recipient { get; set; }
        public string? Des { get; set; }
        public DateTime PayDate { get; set; }
        public Subscription.Subscription? Subscription { get; set; }
    }
}
