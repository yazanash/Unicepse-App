using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Payment;

namespace Uniceps.Core.Models.Subscription
{
    public class Subscription : DomainObject
    {
        public int PlayerId { get; set; }
        public Guid PlayerSyncId { get; set; }
        public int? SportId { get; set; }
        public Guid SportSyncId { get; set; }
        public int? TrainerId { get; set; }
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
        public List<PlayerPayment>? Payments { get; set; } = new();
        public double TotalPaid => Payments?.Sum(p => p.PaymentValue) ?? 0;
        public double Remaining => PriceAfterOffer - TotalPaid;
        public string? Code { get; set; }

        public string GenerateSubscriptionCode(int attempt = 0)
        {
            byte[] bytes = SyncId.ToByteArray();

            int startIndex = attempt % 12;

            int intValue = Math.Abs(BitConverter.ToInt32(bytes, startIndex));

            string code = (intValue % 1000000).ToString("D6");

            return code;
        }

        public void MergeWith(Subscription subscription)
        {
            PlayerId = subscription.PlayerId;
            PlayerSyncId = subscription.PlayerSyncId;
            SportId = subscription.SportId;
            SportSyncId = subscription.SportSyncId;
            TrainerId = subscription.TrainerId;
            TrainerSyncId = subscription.TrainerSyncId;
            PlayerName = subscription.PlayerName;
            SportName = subscription.SportName;
            TrainerName = subscription.TrainerName;
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
            SyncId = subscription.SyncId;
        }
    }
}
