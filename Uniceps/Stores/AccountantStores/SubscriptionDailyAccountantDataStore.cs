using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Stores;

namespace Uniceps.Stores.AccountantStores
{
    public class SubscriptionDailyAccountantDataStore : IDailyAccountantStore<Subscription>
    {
        private readonly IDailyTransactionService<Subscription> _subscriptionsDailyTransactionService;
        private readonly ILogger<GymStore> _logger;
        private readonly List<Subscription> _subscriptions;
        public event Action? SubscriptionsLoaded;
        string LogFlag = "[SDADS] ";
        public SubscriptionDailyAccountantDataStore(IDailyTransactionService<Subscription> subscriptionsDailyTransactionService, ILogger<GymStore> logger)
        {
            _subscriptionsDailyTransactionService = subscriptionsDailyTransactionService;
            _logger = logger;
            _subscriptions = new List<Subscription>();
        }

        public IEnumerable<Subscription> Subscriptions => _subscriptions;
        public async Task GetDaily(DateTime dateTo)
        {
            _logger.LogInformation(LogFlag + "Get daily subscriptions");
            IEnumerable<Subscription> subscriptions = await _subscriptionsDailyTransactionService.GetAll(dateTo);
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            SubscriptionsLoaded?.Invoke();
        }
    }
}
