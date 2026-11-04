using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.API.common
{
    public class SyncSettings
    {
        public string ServerBaseUrl { get; set; } = "http://localhost:5000/";
        public int ConnectionTimeoutSeconds { get; set; } = 10;
        public bool AutoSyncEnabled { get; set; } = false;
        public int SyncIntervalSeconds { get; set; } = 10;
    }
}
