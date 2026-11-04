using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Server.Requests
{
    public class SubscriptionRequest
    {
        public Guid PlayerSyncId { get; set; }
        public Guid SportSyncId { get; set; }
        public Guid? TrainerSyncId { get; set; }
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
        public Subscription ToModel()
        {
            return new Subscription
            {
                PlayerSyncId = PlayerSyncId,
                SportSyncId = SportSyncId,
                TrainerSyncId = TrainerSyncId,
                LastCheck = LastCheck,
                RollDate = RollDate,
                Price = Price,
                OfferValue = OfferValue,
                OfferDes = OfferDes,
                PriceAfterOffer = PriceAfterOffer,
                MonthCount = MonthCount,
                DaysCount = DaysCount,
                IsStopped = IsStopped,
                EndDate = EndDate,
                IsRenewed = IsRenewed,
                Code = Code,
            };
        }
    }
}
