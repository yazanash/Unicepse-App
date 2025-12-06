using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Stores.SystemAuthStores;

namespace Uniceps.Commands.SystemAuthCommands
{
    public class LoadSystemPlansCommand :AsyncCommandBase
    {
        private readonly SystemSubscriptionStore _systemSubscriptionStore;

        public LoadSystemPlansCommand(SystemSubscriptionStore systemSubscriptionStore)
        {
            _systemSubscriptionStore = systemSubscriptionStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _systemSubscriptionStore.GetAllPlans();
        }
    }
}
