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
        public double BalanceForward { get; set; } 
        public double FinalBalance => BalanceForward + Salaries + TotalSubscriptions - Credits;
        public double Salaries { get; set; }
        public List<TrainerDuesDetail> Details { get; set; } = new List<TrainerDuesDetail>();
        public List<Credit> CreditDetails { get; set; } = new List<Credit>();
        public List<SalaryDetail> SalaryDetails { get; set; } = new List<SalaryDetail>();
        public double TotalSalaryDebt { get; set; }
    }
}
