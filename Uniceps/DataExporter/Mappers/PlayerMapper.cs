using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.Mappers
{
    public static class PlayerMapper 
    {
        public static Player FromDto(PlayerDto data)
        {
            Player player = new Player()
            {
                FullName = data.FullName,
                Phone = data.Phone,
                BirthDate = data.BirthDate,
                GenderMale = data.GenderMale,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                Weight = data.Weight,
                Hieght = data.Hieght,
                SubscribeDate = data.SubscribeDate,
                SubscribeEndDate = data.SubscribeEndDate,
                IsTakenContainer = data.IsTakenContainer,
                IsSubscribed = data.IsSubscribed,
            };
            player.Subscriptions = data.Subscriptions.Select(x => SubscriptionMapper.FromDto(x)).ToList();
            return player;
        }

        public static PlayerDto ToDto(Player data)
        {
            PlayerDto playerDto = new PlayerDto()
            {
                FullName = data.FullName,
                Phone = data.Phone,
                BirthDate = data.BirthDate,
                GenderMale = data.GenderMale,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                Weight = data.Weight,
                Hieght = data.Hieght,
                SubscribeDate = data.SubscribeDate,
                SubscribeEndDate = data.SubscribeEndDate,
                IsTakenContainer = data.IsTakenContainer,
                IsSubscribed = data.IsSubscribed,
                
            };
            playerDto.Subscriptions = data.Subscriptions.Select(x => SubscriptionMapper.ToDto(x)).ToList();
            return playerDto;
        }
    }
}
