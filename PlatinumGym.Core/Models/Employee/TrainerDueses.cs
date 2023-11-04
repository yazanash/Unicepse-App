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
        public PlayerPayment? PlayerPayment { get; set; }
        public PlayerTraining? PlayerTraining { get; set; }
        public double Value { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
