using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Subscription;

namespace PlatinumGym.Core.Models.Sport
{
    public class Sport : DomainObject
    {
        public Sport()
        {
            Trainers = new HashSet<Employee.Employee>();
        }
        public string? Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int DaysInWeek { get; set; }
        public double DailyPrice { get; set; }
        public ICollection<Employee.Employee>? Trainers { get; set; } 
        public ICollection<Subscription.Subscription>? PlayerTrainings { get; set; } 
        public int DaysCount { get; set; }
    }
}
