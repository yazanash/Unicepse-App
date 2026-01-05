using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Payment;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.Mappers
{
    public static class PaymentMapper 
    {
        public static PlayerPayment FromDto(PaymentsDto data)
        {
            PlayerPayment payments = new PlayerPayment
            {
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                PlayerSyncId = data.PlayerSyncId,
                PaymentValue = data.PaymentValue,
                Des = data.Des,
                PayDate = data.PayDate,
                SubscriptionSyncId = data.SubscriptionSyncId,
                CoveredFrom = data.CoveredFrom,
                CoveredTo = data.CoveredTo,
            };
            return payments;
        }

        public static PaymentsDto ToDto(PlayerPayment data)
        {
            PaymentsDto paymentsDto = new PaymentsDto
            {
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                PlayerSyncId = data.PlayerSyncId,
                PaymentValue = data.PaymentValue,
                Des = data.Des,
                PayDate = data.PayDate,
                SubscriptionSyncId = data.SubscriptionSyncId,
                CoveredFrom = data.CoveredFrom,
                CoveredTo = data.CoveredTo,
            };
            return paymentsDto;
        }
    }
}
