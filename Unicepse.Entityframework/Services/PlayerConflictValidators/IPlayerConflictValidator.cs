using Unicepse.Core.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Entityframework.Services.PlayerConflictValidators
{
    public interface IPlayerConflictValidator
    {

        Task<Player> GetConflicting(Player player);
    }
}
