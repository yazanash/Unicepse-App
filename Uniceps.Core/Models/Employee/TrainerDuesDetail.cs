using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.Employee
{
    public class TrainerDuesDetail
    {
        public int SubscriptionId { get; set; }
        public string? PlayerName { get; set; }
        public string? SportName { get; set; }
        public double PaymentValue { get; set; }
        public DateTime CoveredFrom { get; set; }
        public DateTime CoveredTo { get; set; }
        public double AmountForMonth { get; set; }
        public bool IsLatePayment { get; set; } = false;
    }
}
