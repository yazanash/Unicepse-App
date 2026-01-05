using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Models;
using Uniceps.API.ResponseModels;
using Uniceps.Core.Models.SystemAuthModels;

namespace Uniceps.API.Services
{
    public class SystemSubscriptionApiDataService
    {
        private readonly UnicepseApiClientV2 _client;

        public SystemSubscriptionApiDataService(UnicepseApiClientV2 client)
        {
            _client = client;
        }
        public async Task<ApiResponse<MembershipPaymentResponse>> Create(SystemPlanItem systemPlanModel)
        {
            SystemPlanModelDto systemPlanModelDto = new()
            {
                PlanItemId = systemPlanModel.Id,
            };
            return await _client.PostAsync<SystemPlanModelDto, MembershipPaymentResponse>("Membership", systemPlanModelDto);
        }
        public async Task<ApiResponse<SystemSubscriptionResponse>> GetActiveSubscription()
        {
            return await _client.GetAsync<SystemSubscriptionResponse>("Membership/1");
        }
        public async Task<ApiResponse<List<SystemPlanResponse>>> GetPlans()
        {
            return await _client.GetAsync<List<SystemPlanResponse>>("Plan/1");
        }
    }
}
