using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Sub = Uniceps.Core.Models.Subscription;

namespace Uniceps.Core.Models.Payment
{
    public class PlayerPayment : DomainObject
    {
        public int PlayerId { get; set; }
        public Guid PlayerSyncId { get; set; }
        public virtual Player.Player? Player { get; set; }
        public double PaymentValue { get; set; }
        [NotMapped]
        public virtual Employee.Employee? Recipient { get; set; }
        public string? Des { get; set; }
        public DateTime PayDate { get; set; }
        public int SubscriptionId { get; set; }
        public Guid SubscriptionSyncId { get; set; }
        public Sub.Subscription? Subscription { get; set; }
        public DateTime CoveredFrom { get; set; }
        public DateTime CoveredTo { get; set; }
        public int CoveredDays => (CoveredTo - CoveredFrom).Days + 1;

        public void MergeWith(PlayerPayment playerPayment)
        {
            PlayerId = playerPayment.PlayerId;
            PlayerSyncId = playerPayment.PlayerSyncId;
            PaymentValue = playerPayment.PaymentValue;
            Des = playerPayment.Des;
            PayDate = playerPayment.PayDate;
            SubscriptionId = playerPayment.SubscriptionId;
            SubscriptionSyncId = playerPayment.SubscriptionSyncId;
            CoveredFrom = playerPayment.CoveredFrom;
            CoveredTo = playerPayment.CoveredTo;
            SyncId = playerPayment.SyncId;
        }
    }
}
