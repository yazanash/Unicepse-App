using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.API.SyncServices
{
    public interface IServerChanges<TRequest,TResponse>
    {
        Task PushLocalChangesAsync(List<TRequest> data);
        Task<IEnumerable<TResponse>> PulServerChangesAsync(long data);
    }
}
