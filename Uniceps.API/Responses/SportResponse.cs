using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Sport;

namespace Uniceps.API.Responses
{
    public class SportResponse
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int DaysInWeek { get; set; }
        public int DaysCount { get; set; }
        public long ServerVersion { get; set; }
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
                SyncId = SyncId,
                ServerVersion = ServerVersion
            };
        }
    }
}
