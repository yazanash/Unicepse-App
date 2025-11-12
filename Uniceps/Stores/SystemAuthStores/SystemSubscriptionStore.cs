using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.API.common;
using Uniceps.API.ResponseModels;
using Uniceps.API.Services;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.Core.Services;

namespace Uniceps.Stores.SystemAuthStores
{
    public class SystemSubscriptionStore
    {
        private readonly SystemSubscriptionApiDataService _subscriptionApiDataService;
        private readonly ISystemSubscriptionDataService _systemSubscriptionDataService;
        public SystemSubscriptionStore(SystemSubscriptionApiDataService subscriptionApiDataService, ISystemSubscriptionDataService systemSubscriptionDataService)
        {
            _subscriptionApiDataService = subscriptionApiDataService;
            _systemPlans = new List<SystemPlanModel>();
            _systemSubscriptionDataService = systemSubscriptionDataService;
        }
        private List<SystemPlanModel> _systemPlans;
        public IEnumerable<SystemPlanModel> SystemPlanModels => _systemPlans;
        public event Action<SystemPlanModel>? Created;
        public event Action? Loaded;
        public event Action<SystemPlanModel>? Updated;
        public event Action<int>? Deleted;

        public async Task Add(SystemPlanItem entity)
        {
            ApiResponse<MembershipPaymentResponse> apiResponse = await _subscriptionApiDataService.Create(entity);
            if (apiResponse.StatusCode == 201 || apiResponse.StatusCode == 200)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = apiResponse.Data!.PaymentUrl,
                        UseShellExecute = true // This ensures it opens in the default browser
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open browser: " + ex.Message);
                }

            }
        }

        public Task Delete(int entity_id)
        {
            throw new NotImplementedException();
        }

        public async Task GetAllPlans()
        {
            ApiResponse<List<SystemPlanResponse>> systemPlanResponse = await _subscriptionApiDataService.GetPlans();
            foreach (var plan in systemPlanResponse.Data!)
            {
                SystemPlanModel planModel = new SystemPlanModel()
                {
                    Id = plan.Id,
                    Name = plan.Name,
                };
                foreach (var item in plan.PlanItems)
                {
                    SystemPlanItem itemModel = new SystemPlanItem()
                    {
                        Id = item.Id,
                        DurationString = item.DurationString,
                        Price = item.Price,
                        DaysCount = item.DaysCount,
                        IsFree = item.IsFree
                    };
                    planModel.PlanItems.Add(itemModel);
                }
                _systemPlans.Add(planModel);
            }
            Loaded?.Invoke();
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public Task Update(SystemPlanModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
