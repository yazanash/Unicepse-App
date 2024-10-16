using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BackgroundServiceStore> _logger;
        string LogFlag = "[Background] ";
        public BackgroundServiceStore(PlayersDataStore playersDataStore, SubscriptionDataStore subscriptionDataStore,
            PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore, ILogger<BackgroundServiceStore> logger)
        {
            _playersDataStore = playersDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _logger = logger;
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
            _logger.LogInformation(LogFlag+"sync players creation started");
            await _playersDataStore.SyncPlayersToCreate();
            _logger.LogInformation(LogFlag + "sync players updates started");
            await _playersDataStore.SyncPlayersToUpdate();
        }
        public async Task SyncSubscribtions()
        {
            _logger.LogInformation(LogFlag + "sync subscriptions creation started");
            await _subscriptionDataStore.SyncSubscriptionsToCreate();
            _logger.LogInformation(LogFlag + "sync subscriptions updates started");
            await _subscriptionDataStore.SyncSubscriptionsToUpdate();
        }

        public async Task SyncPayments()
        {
            _logger.LogInformation(LogFlag + "sync payments creation started");
            await _paymentDataStore.SyncPaymentsToCreate();
            _logger.LogInformation(LogFlag + "sync payments updates started");
            await _paymentDataStore.SyncPaymentsToUpdate();
            _logger.LogInformation(LogFlag + "sync payments deletion started");
            await _paymentDataStore.SyncPaymentsToDelete();
        }
        public async Task SyncMetrics()
        {
            _logger.LogInformation(LogFlag + "sync metrics creation started");
            await _metricDataStore.SyncMetricsToCreate();
            _logger.LogInformation(LogFlag + "sync metrics updates started");
            await _metricDataStore.SyncMetricsToUpdate();
        }
        public async Task SyncRoutines()
        {
            _logger.LogInformation(LogFlag + "sync routines creation started");
            await _routineDataStore.SyncRoutineToCreate();
            _logger.LogInformation(LogFlag + "sync routines updates started");
            await _routineDataStore.SyncRoutineToUpdate();
        }
        public async Task SyncAttendances()
        {
            _logger.LogInformation(LogFlag + "sync attendances creation started");
            await _playersAttendenceStore.SyncAttendanceToCreate();
            _logger.LogInformation(LogFlag + "sync attendances updates started");
            await _playersAttendenceStore.SyncAttendanceToUpdate();
        }
    }
}
