using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Server.Services
{
    public interface IPushPullDataService<T>
    {
        Task<IEnumerable<T>> PullDataAsync(long lastServerVersion);
        Task PushDataAsync(List<T> data);
    }
}
