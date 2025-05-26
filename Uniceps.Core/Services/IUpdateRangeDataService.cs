using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Services
{
    public interface IUpdateRangeDataService<T>
    {
        Task<IEnumerable<T>> UpdateRange(List<T> entity);
    }
}
