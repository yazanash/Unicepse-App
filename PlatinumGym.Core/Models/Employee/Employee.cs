using PlatinumGym.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Core.Models.Sport;

namespace PlatinumGym.Core.Models.Employee
{
    public class Employee:Person
    {
      
        public bool Salary { get; set; }
        public bool Parcent { get; set; }
        public double SalaryValue { get; set; }
        public int ParcentValue { get; set; }
        public bool IsSecrtaria { get; set; }
        public string? Position { get; set; }
        public DateTime StartDate { get; set; }
        public double Balance { get; set; }
        public bool IsActive { get; set; }
        public bool IsTrainer { get; set; }
        public ICollection<Sport.Sport>? Sports { get; set; }
        public ICollection<Subscription.Subscription>? PlayerTrainings { get; set; }

        public Employee()
        {
            Sports = new HashSet<Sport.Sport>();
            PlayerTrainings=new HashSet<Subscription.Subscription>();
        }
    }
}
