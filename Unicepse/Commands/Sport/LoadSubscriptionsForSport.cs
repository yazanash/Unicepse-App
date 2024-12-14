using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.Stores.SportStores;

namespace Unicepse.Commands.Sport
{
    public class LoadSubscriptionsForSport : AsyncCommandBase
    {
        private readonly SportDataStore  _sportDataStore;
        private readonly SportSubscriptionDataStore _subscriptionDataStore;

        public LoadSubscriptionsForSport(SportDataStore sportDataStore, SportSubscriptionDataStore subscriptionDataStore)
        {
            _sportDataStore = sportDataStore;
            _subscriptionDataStore = subscriptionDataStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            await _subscriptionDataStore.GetAll(_sportDataStore.SelectedSport!, DateTime.Now);
        }
    }
}
