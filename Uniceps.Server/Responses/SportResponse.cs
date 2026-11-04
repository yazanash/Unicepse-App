using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Sport;

namespace Uniceps.Server.Responses
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
        public SportResponse(Sport sport)
        {
            Name = sport.Name;
            Price = sport.Price;
            IsActive = sport.IsActive;
            DaysCount = sport.DaysCount;
            DaysInWeek = sport.DaysInWeek;
            ServerVersion = sport.ServerVersion;
            SyncId = sport.SyncId;
        }
    }
}
