using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class Sport : DomainObject
    {
        public Sport()
        {
            Trainers = new HashSet<Employee>();
        }
        public string? Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int DaysInWeek { get; set; }
        public double DailyPrice { get; set; }
        public ICollection<Employee>? Trainers { get; set; } 
        public ICollection<PlayerTraining>? PlayerTrainings { get; set; } 
        public int DaysCount { get; set; }
    }
}
