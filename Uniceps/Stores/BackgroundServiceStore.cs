using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Stores.ApiDataStores;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.Stores
{
    public class BackgroundServiceStore
    {
        private readonly ILogger<BackgroundServiceStore> _logger;
        private readonly SyncStore _syncStore;
        public BackgroundServiceStore( ILogger<BackgroundServiceStore> logger, SyncStore syncStore)
        {
            _logger = logger;
            _syncStore = syncStore;
        }
        private string? _backMessage;
        public string? BackMessage
        {
            get { return _backMessage; }
            set { _backMessage = value; }
        }
        private bool _connection;
        public bool Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
        private bool _syncStateProp;
        public bool SyncStateProp
        {
            get { return _syncStateProp; }
            set { _syncStateProp = value; }
        }
        private string? _syncMessage;
        public string? SyncMessage
        {
            get { return _syncMessage; }
            set { _syncMessage = value; }
        }
        public event Action<string?, bool>? StateChanged;
        public event Action<bool, string?>? SyncStatus;

        public void ChangeState(string message, bool connectionStatus)
        {
            BackMessage = message;
            Connection = connectionStatus;
            StateChanged?.Invoke(message, connectionStatus);
        }
        public void SyncState(bool connectionStatus, string message)
        {
            SyncMessage = message;
            SyncStateProp = connectionStatus;
            SyncStatus?.Invoke(connectionStatus, message);
        }
        public async Task Sync()
        {
            IEnumerable<SyncObject> syncObjects = await _syncStore.GetAll();
            foreach (SyncObject syncObject in syncObjects)
            {
              
            }
        }
    }
}
