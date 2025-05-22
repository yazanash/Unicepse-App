using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Stores;

namespace Uniceps.Stores.SportStores
{
    public class SportSubscriptionDataStore
    {
        private readonly ISportMonthlyTransactions<Subscription> _sportMonthlyTransactions;
        private readonly ILogger<SubscriptionDataStore> _logger;
        string LogFlag = "[Subscriptions] ";
        private readonly List<Subscription> _subscriptions;

        public SportSubscriptionDataStore(ISportMonthlyTransactions<Subscription> sportMonthlyTransactions, ILogger<SubscriptionDataStore> logger)
        {
            _sportMonthlyTransactions = sportMonthlyTransactions;
            _logger = logger;
            _subscriptions = new List<Subscription>();
        }

        public IEnumerable<Subscription> Subscriptions => _subscriptions;
        public event Action? Loaded;
        public async Task GetAll(Sport sport, DateTime date)
        {
            _logger.LogInformation(LogFlag + "get all subscription for sport in date {0}", date);
            IEnumerable<Subscription> subscriptions = await _sportMonthlyTransactions.GetAll(sport, date);
            _subscriptions.Clear();
            _subscriptions.AddRange(subscriptions);
            Loaded?.Invoke();
        }
    }
}
