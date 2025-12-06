using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;

namespace Uniceps.Core.Models.Employee
{
    public class TrainerDueses : DomainObject
    {
        public Employee? Trainer { get; set; }
        public double TotalSubscriptions { get; set; }
        public int CountSubscription { get; set; }
        public DateTime IssueDate { get; set; }
        public double Parcent { get; set; }
        public double Credits { get; set; }
        public double Salary { get; set; }
        public double CreditsCount { get; set; }
        public List<TrainerDuesDetail> Details { get; set; } = new List<TrainerDuesDetail>();
    }
}
