using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Services;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.Core.Services;

namespace Uniceps.Helpers
{
    public class SystemSubscriptionManager
    {
        private readonly ISystemSubscriptionDataService _systemSubscriptionDataService;
        private readonly SystemSubscriptionApiDataService _systemSubscriptionApiDataService;

        public SystemSubscriptionManager(ISystemSubscriptionDataService systemSubscriptionDataService, SystemSubscriptionApiDataService systemSubscriptionApiDataService)
        {
            _systemSubscriptionDataService = systemSubscriptionDataService;
            _systemSubscriptionApiDataService = systemSubscriptionApiDataService;
        }
        public async Task<bool> CheckAndSyncSubscriptionAsync()
        {
            var localsystemSubscription = await _systemSubscriptionDataService.GetActiveSubscription();

            var remotesystemSubscription = await _systemSubscriptionApiDataService.GetActiveSubscription();
            if (remotesystemSubscription.Data != null && remotesystemSubscription.StatusCode == 200)
            {
                SystemSubscription systemSubscription = new SystemSubscription()
                {
                    PublicId = remotesystemSubscription.Data.Id,
                    PlanName = remotesystemSubscription.Data.Plan,
                    StartDate = remotesystemSubscription.Data.StartDate,
                    EndDate = remotesystemSubscription.Data.EndDate,
                    IsActive = remotesystemSubscription.Data.IsActive,
                    IsGift = remotesystemSubscription.Data.IsGift,
                    Price = remotesystemSubscription.Data.Price
                };
                if (localsystemSubscription != null)
                {
                    systemSubscription.Id = localsystemSubscription.Id;
                    await _systemSubscriptionDataService.Update(systemSubscription);
                }
                else
                    await _systemSubscriptionDataService.Create(systemSubscription);

                return true;
            }

            return false;
        }

    }
}
