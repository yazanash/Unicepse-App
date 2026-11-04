using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Sport;

namespace Uniceps.API.Requests
{
    public class SportRequest
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int DaysInWeek { get; set; }
        public int DaysCount { get; set; }
        public Guid SyncId { get; set; }
        public SportRequest(Sport sport)
        {
            Name = sport.Name;
            Price = sport.Price;
            IsActive = sport.IsActive;
            DaysCount = sport.DaysCount;
            DaysInWeek = sport.DaysInWeek;
            SyncId = sport.SyncId;
        }
    }
}
