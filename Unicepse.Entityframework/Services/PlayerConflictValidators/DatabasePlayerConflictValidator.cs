using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.PlayerConflictValidators
{
    public class DatabasePlayerConflictValidator : IPlayerConflictValidator
    {
        private readonly PlatinumGymDbContextFactory _platinumGymDbContext;

        public DatabasePlayerConflictValidator(PlatinumGymDbContextFactory platinumGymDbContext)
        {
            _platinumGymDbContext = platinumGymDbContext;
        }

       

        public async Task<Player> GetConflicting(Player player)
        {
            using PlatinumGymDbContext context = _platinumGymDbContext.CreateDbContext();
            
              Player? playerDTO=  await context.Players!.Where(r=>r.FullName!.Equals(player.FullName)).FirstOrDefaultAsync();
                if (playerDTO == null)
                    return null!;
                return playerDTO;
            
        }

       

        //public Task<Player> GetConflicting(Player player)
        //{
        //    throw new NotImplementedException();
        //}

        //static Player ToPlayer(PlayerDTO playerDTO)
        //{
        //    return new Player()
        //    {
        //        Id = playerDTO.Id,
        //        Balance = playerDTO.Balance,
        //        BirthDate = playerDTO.BirthDate,
        //        FullName = playerDTO.FullName,
        //        GenderMale = playerDTO.GenderMale,
        //        Hieght = playerDTO.Hieght,
        //        IsSubscribed = playerDTO.IsSubscribed,
        //        IsTakenContainer = playerDTO.IsTakenContainer,
        //        Phone = playerDTO.Phone,
        //        SubscribeDate = playerDTO.SubscribeDate,
        //        SubscribeEndDate = playerDTO.SubscribeEndDate,
        //        Weight = playerDTO.Weight,
        //    };
        //}
    }
}
