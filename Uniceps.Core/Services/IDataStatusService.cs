using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;

namespace Uniceps.Core.Services
{
    public interface IDataStatusService<T>
    {
        Task<IEnumerable<T>> GetByDataStatus(DataStatus status);
        Task<T> UpdateDataStatus(T entity);
    }
}
