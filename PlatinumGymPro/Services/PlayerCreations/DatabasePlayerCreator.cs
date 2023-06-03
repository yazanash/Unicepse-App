using PlatinumGymPro.DbContexts;
using PlatinumGymPro.DTOs;
using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Services.PlayerCreations
{
    public class DatabasePlayerCreator : IPlayerCreator
    {
        private readonly PlatinumGymDbContextFactory _platinumGymDbContext;

        public DatabasePlayerCreator(PlatinumGymDbContextFactory platinumGymDbContext)
        {
            _platinumGymDbContext = platinumGymDbContext;
        }

        public async Task Create(Player player)
        {
            using PlatinumGymDbContext context = _platinumGymDbContext.CreateDbContext();
            
                Player playerDTO = player;
                context.Players!.Add(playerDTO);
                await context.SaveChangesAsync();
            
        }

        //private PlayerDTO ToPlayerDTO(Player player)
        //{
        //    return new PlayerDTO()
        //    {
        //        Balance = player.Balance,
        //        BirthDate = player.BirthDate,
        //        FullName = player.FullName,
        //        GenderMale = player.GenderMale,
        //        Hieght = player.Hieght,
        //        IsSubscribed = player.IsSubscribed,
        //        IsTakenContainer = player.IsTakenContainer,
        //        Phone = player.Phone,
        //        SubscribeDate = player.SubscribeDate,
        //        SubscribeEndDate = player.SubscribeEndDate,
        //        Weight = player.Weight,
        //    };
        //}
    }
}
