using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.SubscriptionCommand
{
    public class CreateSubscriptionCommand : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly NavigationStore _navigationStore;

        public CreateSubscriptionCommand(SubscriptionDataStore subscriptionDataStore, NavigationStore navigationStore)
        {
            _subscriptionDataStore = subscriptionDataStore;
            _navigationStore = navigationStore;
        }

        public override Task ExecuteAsync(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
