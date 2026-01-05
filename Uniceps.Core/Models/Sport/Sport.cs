using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Emp = Uniceps.Core.Models.Employee;
using Sub = Uniceps.Core.Models.Subscription;

namespace Uniceps.Core.Models.Sport
{
    public class Sport : DomainObject
    {
        public Sport()
        {
            Trainers = new HashSet<Emp.Employee>();
        }
        public string? Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int DaysInWeek { get; set; }
        public ICollection<Emp.Employee>? Trainers { get; set; }
        public ICollection<Sub.Subscription>? PlayerTrainings { get; set; }
        public int DaysCount { get; set; }

        public void MergeWith(Sport sport)
        {
            Name = sport.Name;
            Price = sport.Price;
            IsActive = sport.IsActive;
            DaysInWeek = sport.DaysInWeek;
            DaysCount = sport.DaysCount;
            SyncId = sport.SyncId;
        }
    }
}
