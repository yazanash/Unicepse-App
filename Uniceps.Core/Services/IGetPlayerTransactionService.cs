using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;

namespace Uniceps.Core.Services
{
    public interface IGetPlayerTransactionService<T>
    {
        Task<IEnumerable<T>> GetAll(Player player);
    }

}
