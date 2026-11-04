using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Server.Requests
{
    public class SportRequest
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int DaysInWeek { get; set; }
        public int DaysCount { get; set; }
        public Guid SyncId { get; set; }
        public Sport ToModel()
        {
            return new Sport
            {
                Name = Name,
                Price = Price,
                IsActive = IsActive,
                DaysCount = DaysCount,
                DaysInWeek = DaysInWeek,
                SyncId = SyncId

            };
        }
    }
}
