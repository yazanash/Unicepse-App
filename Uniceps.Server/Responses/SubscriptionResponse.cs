using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Server.Responses
{
    public class SubscriptionResponse
    {
        public SubscriptionResponse(Subscription subscription)
        {
            PlayerSyncId = subscription.PlayerSyncId;
            SportSyncId = subscription.SportSyncId;
            TrainerSyncId = subscription.TrainerSyncId;
            LastCheck = subscription.LastCheck;
            RollDate = subscription.RollDate;
            Price = subscription.Price;
            OfferValue = subscription.OfferValue;
            OfferDes = subscription.OfferDes;
            PriceAfterOffer = subscription.PriceAfterOffer;
            MonthCount = subscription.MonthCount;
            DaysCount = subscription.DaysCount;
            IsStopped = subscription.IsStopped;
            EndDate = subscription.EndDate;
            IsRenewed = subscription.IsRenewed;
            Code = subscription.Code;
        }
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
        public string? Code { get; set; }
    }
}
