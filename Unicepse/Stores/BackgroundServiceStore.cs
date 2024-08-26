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

        public BackgroundServiceStore(PlayersDataStore playersDataStore, SubscriptionDataStore subscriptionDataStore,
            PaymentDataStore paymentDataStore, MetricDataStore metricDataStore)
        {
            _playersDataStore = playersDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
        }

        public event Action<string?,bool>? StateChanged;

        public void ChangeState(string message , bool connectionStatus)
        {
            StateChanged?.Invoke(message, connectionStatus);
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
    }
}
