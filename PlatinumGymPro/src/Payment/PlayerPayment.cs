using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class PlayerPayment : DomainObject
    {
        public virtual Player? Player { get; set; }
        public double PaymentValue { get; set; }
        public virtual Employee? Recipient { get; set; }
        public string? Des { get; set; }
        public DateTime PayDate { get; set; }
        public PlayerTraining? Training { get; set; }
    }
}
