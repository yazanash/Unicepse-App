using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;

namespace Uniceps.DataExporter.Dtos
{
    public class PaymentsDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid SyncId { get; set; }
        public int PlayerId { get; set; }
        public Guid PlayerSyncId { get; set; }
        public double PaymentValue { get; set; }
        public string? Des { get; set; }
        public DateTime PayDate { get; set; }
        public int SubscriptionId { get; set; }
        public Guid SubscriptionSyncId { get; set; }
        public DateTime CoveredFrom { get; set; }
        public DateTime CoveredTo { get; set; }
    }
}
