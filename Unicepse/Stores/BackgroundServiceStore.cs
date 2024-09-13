using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Stores
{
    public class BackgroundServiceStore
    {
        private readonly PlayersDataStore _playersDataStore;
        private readonly SubscriptionDataStore  _subscriptionDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly MetricDataStore _metricDataStore;
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;

        public BackgroundServiceStore(PlayersDataStore playersDataStore, SubscriptionDataStore subscriptionDataStore,
            PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore)
        {
            _playersDataStore = playersDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _playersAttendenceStore = playersAttendenceStore;
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
        public async Task SyncPlayers()
        {
            await _playersDataStore.SyncPlayersToCreate();
            await _playersDataStore.SyncPlayersToUpdate();
        }
        public async Task SyncSubscribtions()
        {
            await _subscriptionDataStore.SyncSubscriptionsToCreate();
            await _subscriptionDataStore.SyncSubscriptionsToUpdate();
        }

        public async Task SyncPayments()
        {
            await _paymentDataStore.SyncPaymentsToCreate();
            await _paymentDataStore.SyncPaymentsToUpdate();
            await _paymentDataStore.SyncPaymentsToDelete();
        }
        public async Task SyncMetrics()
        {
            await _metricDataStore.SyncMetricsToCreate();
            await _metricDataStore.SyncMetricsToUpdate();
        }
        public async Task SyncRoutines()
        {
            await _routineDataStore.SyncRoutineToCreate();
            await _routineDataStore.SyncRoutineToUpdate();
        }
        public async Task SyncAttendances()
        {
            await _playersAttendenceStore.SyncAttendanceToCreate();
            await _playersAttendenceStore.SyncAttendanceToUpdate();
        }
    }
}
