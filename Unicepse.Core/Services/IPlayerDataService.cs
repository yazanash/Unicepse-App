using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models.Player;

namespace Unicepse.Core.Services
{
    public interface IPlayerDataService : IDataService<Player>
    {
        Task<IEnumerable<Player>> GetByStatus(bool status);
        Task<Player?> GetByUID(string uid);
        Task<IEnumerable<Player>> GetByDataStatus(DataStatus status);
        Task<Player> UpdateDataStatus(Player entity);
    }
}
