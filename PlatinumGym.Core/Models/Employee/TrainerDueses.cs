using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Payment;
using PlatinumGym.Core.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Core.Models.Employee
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
    }
}
