using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Sport;

namespace Uniceps.Core.Services
{
    public interface ISportMonthlyTransactions<T>
    {
        Task<IEnumerable<T>> GetAll(Sport sport, DateTime date);
    }
}
