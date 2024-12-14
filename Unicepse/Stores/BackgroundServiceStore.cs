using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models.DailyActivity;
using Unicepse.Core.Models.Metric;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Models.SyncModel;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Stores.ApiDataStores;

namespace Unicepse.Stores
{
    public class BackgroundServiceStore
    {
        private readonly IApiDataStore<Player> _playersApiDataStore;
        private readonly IApiDataStore<Subscription> _subscriptionApiDataStore;
        private readonly IApiDataStore<PlayerPayment> _paymentApiDataStore;
        private readonly IDeleteApiDataStore<PlayerPayment> _deletePaymentApiDataStore;

        private readonly IApiDataStore<Metric> _metricApiDataStore;
        private readonly IApiDataStore<PlayerRoutine> _routineApiDataStore;
        private readonly IApiDataStore<DailyPlayerReport> _AttendenceApiDataStore;
        private readonly ILogger<BackgroundServiceStore> _logger;
        private readonly SyncStore _syncStore;
        string LogFlag = "[Background] ";
        public BackgroundServiceStore(IApiDataStore<Player> playersApiDataStore, IApiDataStore<Subscription> subscriptionApiDataStore,
            IApiDataStore<PlayerPayment> paymentApiDataStore, IApiDataStore<Metric> metricApiDataStore, IApiDataStore<PlayerRoutine> routineApiDataStore,
            IApiDataStore<DailyPlayerReport> AttendenceApiDataStore, ILogger<BackgroundServiceStore> logger, IDeleteApiDataStore<PlayerPayment> deletePaymentApiDataStore, SyncStore syncStore)
        {
            _playersApiDataStore = playersApiDataStore;
            _subscriptionApiDataStore = subscriptionApiDataStore;
            _paymentApiDataStore = paymentApiDataStore;
            _metricApiDataStore = metricApiDataStore;
            _routineApiDataStore = routineApiDataStore;
            _AttendenceApiDataStore = AttendenceApiDataStore;
            _logger = logger;
            _deletePaymentApiDataStore = deletePaymentApiDataStore;
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
            set { _syncStateProp = value;  }
        }
        private string? _syncMessage;
        public string? SyncMessage
        {
            get { return _syncMessage; }
            set { _syncMessage = value; }
        }
        public event Action<string?,bool>? StateChanged;
        public event Action<bool, string?>? SyncStatus;

        public void ChangeState(string message , bool connectionStatus)
        {
            BackMessage = message;
            Connection = connectionStatus;
            StateChanged?.Invoke(message, connectionStatus);
        }
        public void SyncState(bool connectionStatus, string message)
        {
            SyncMessage = message;
            SyncStateProp = connectionStatus;
            SyncStatus?.Invoke(connectionStatus,message);
        }
        public async Task Sync()
        {
            IEnumerable<SyncObject> syncObjects = await _syncStore.GetAll();
            foreach(SyncObject syncObject in syncObjects)
            {
                switch (syncObject.EntityType)
                {
                    case DataType.Player:
                        await _playersApiDataStore.Sync(syncObject);
                        break;
                    case DataType.Subscription:
                        await _subscriptionApiDataStore.Sync(syncObject);
                        break;
                    case DataType.Payment:
                        await _paymentApiDataStore.Sync(syncObject);
                        break;
                    case DataType.Metric:
                        await _metricApiDataStore.Sync(syncObject);
                        break;
                    case DataType.Routine:
                        await _routineApiDataStore.Sync(syncObject);
                        break;
                    case DataType.Attendance:
                        await _AttendenceApiDataStore.Sync(syncObject);
                        break;
                }
            }
        }
    }
}
