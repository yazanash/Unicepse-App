using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.DataExporter.Dtos
{
    public class SubscriptionDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid SyncId { get; set; }
        public Guid PlayerSyncId { get; set; }
        public Guid SportSyncId { get; set; }
        public Guid? TrainerSyncId { get; set; }
        public string? PlayerName { get; set; } = string.Empty;
        public string? SportName { get; set; } = string.Empty;
        public string? TrainerName { get; set; }
        public DateTime LastCheck { get; set; }
        public DateTime RollDate { get; set; }
        public double Price { get; set; }
        public double OfferValue { get; set; }
        public string? OfferDes { get; set; }
        public double PriceAfterOffer { get; set; }
        public int MonthCount { get; set; }
        public int DaysCount { get; set; }
        public bool IsStopped { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsRenewed { get; set; }
        public List<PaymentsDto>? Payments { get; set; } = new();
        public string? Code { get; set; }
    }
}
