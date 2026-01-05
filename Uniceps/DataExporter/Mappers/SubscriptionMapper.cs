using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.Mappers
{
    public static class SubscriptionMapper 
    {
        public static Subscription FromDto(SubscriptionDto data)
        {
            Subscription subscription = new Subscription
            {
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                PlayerSyncId = data.PlayerSyncId,
                SportSyncId = data.SportSyncId,
                TrainerSyncId = data.TrainerSyncId,
                PlayerName = data.PlayerName,
                SportName = data.SportName,
                TrainerName = data.TrainerName,
                LastCheck = data.LastCheck,
                RollDate = data.RollDate,
                Price = data.Price,
                OfferValue = data.OfferValue,
                OfferDes = data.OfferDes,
                PriceAfterOffer = data.PriceAfterOffer,
                MonthCount = data.MonthCount,
                DaysCount = data.DaysCount,
                IsStopped = data.IsStopped,
                EndDate = data.EndDate,
                IsRenewed = data.IsRenewed,
                Code = data.Code,
            };
            subscription.Payments = data.Payments?.Select(x => PaymentMapper.FromDto(x)).ToList();
            return subscription;
        }

        public static SubscriptionDto ToDto(Subscription data)
        {
            SubscriptionDto subscriptionDto = new SubscriptionDto
            {
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                PlayerSyncId = data.PlayerSyncId,
                SportSyncId = data.SportSyncId,
                TrainerSyncId = data.TrainerSyncId,
                PlayerName = data.PlayerName,
                SportName = data.SportName,
                TrainerName = data.TrainerName,
                LastCheck = data.LastCheck,
                RollDate = data.RollDate,
                Price = data.Price,
                OfferValue = data.OfferValue,
                OfferDes = data.OfferDes,
                PriceAfterOffer = data.PriceAfterOffer,
                MonthCount = data.MonthCount,
                DaysCount = data.DaysCount,
                IsStopped = data.IsStopped,
                EndDate = data.EndDate,
                IsRenewed = data.IsRenewed,
                Code = data.Code,
            };
            subscriptionDto.Payments = data.Payments?.Select(x => PaymentMapper.ToDto(x)).ToList();
            return subscriptionDto;
        }
    }
}
