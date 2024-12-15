using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;

namespace Unicepse.Core.Services
{
    public interface IGetPlayerTransactionService<T>
    {
        Task<IEnumerable<T>> GetAll(Player player);
    }

}
