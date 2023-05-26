using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Services.PlayersProviders
{
    public interface IPlayerProvider
    {
        Task<IEnumerable<Player>> GetAllPlayers();
    }
}
