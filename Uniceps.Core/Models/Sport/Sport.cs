using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using emp = Uniceps.Core.Models.Employee;
using sub = Uniceps.Core.Models.Subscription;

namespace Uniceps.Core.Models.Sport
{
    public class Sport : DomainObject
    {
        public Sport()
        {
            Trainers = new HashSet<emp.Employee>();
        }
        public string? Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int DaysInWeek { get; set; }
        public double DailyPrice { get; set; }
        public ICollection<emp.Employee>? Trainers { get; set; }
        public ICollection<sub.Subscription>? PlayerTrainings { get; set; }
        public int DaysCount { get; set; }
    }
}
