using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uniceps.BackgroundServices.SyncServices
{
    public interface ISyncEngine
    {
        public int Priority { get; }
        string Name { get; }
        Task SyncAsync(CancellationToken cancellationToken = default);
    }
}
