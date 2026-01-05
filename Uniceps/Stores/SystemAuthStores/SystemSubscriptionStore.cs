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
using Uniceps.BackgroundServices;
using Uniceps.Core.Models;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.Core.Services;

namespace Uniceps.Stores.SystemAuthStores
{
    public class SystemSubscriptionStore
    {
        private readonly SystemSubscriptionApiDataService _subscriptionApiDataService;
        private readonly ISystemSubscriptionDataService _systemSubscriptionDataService;
        private readonly AccountStore _accountStore;
        public SystemSubscriptionStore(SystemSubscriptionApiDataService subscriptionApiDataService, ISystemSubscriptionDataService systemSubscriptionDataService, AccountStore accountStore)
        {
            _subscriptionApiDataService = subscriptionApiDataService;
            _systemPlans = new List<SystemPlanModel>();
            _systemSubscriptionDataService = systemSubscriptionDataService;
            _accountStore = accountStore;
        }
        private List<SystemPlanModel> _systemPlans;
        public IEnumerable<SystemPlanModel> SystemPlanModels => _systemPlans;
        public event Action? Loaded;
        public event Action? Created;
        public async Task<MembershipPayment> Add(SystemPlanItem entity)
        {
            ApiResponse<MembershipPaymentResponse> apiResponse = await _subscriptionApiDataService.Create(entity);
            if (apiResponse.StatusCode == 201 || apiResponse.StatusCode == 200)
            {
                try
                {
                    MembershipPayment membershipPayment = new MembershipPayment();
                    membershipPayment.RequirePayment = apiResponse.Data!.RequirePayment;
                    membershipPayment.PaymentUrl = apiResponse.Data!.PaymentUrl;
                    membershipPayment.CashPaymentUrl = apiResponse.Data!.CashPaymentUrl;
                    membershipPayment.Message = apiResponse.Data!.Message;
                    Created?.Invoke();
                    if (!membershipPayment.RequirePayment)
                        await CheckAndSyncSubscriptionAsync();
                    return membershipPayment;


                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to open browser: " + ex.Message);
                }

            }
            else
            {
                throw new Exception("خطأ في شراء النسخة");
            }
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
        public async Task<bool> CheckAndSyncSubscriptionAsync()
        {
            var localsystemSubscription = await _systemSubscriptionDataService.GetActiveSubscription();
            try
            {

                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    var remotesystemSubscription = await _subscriptionApiDataService.GetActiveSubscription();
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
                            _accountStore.SystemSubscription = await _systemSubscriptionDataService.Update(systemSubscription);
                        }
                        else
                            _accountStore.SystemSubscription = await _systemSubscriptionDataService.Create(systemSubscription);

                        return true;
                    }
                    else if (remotesystemSubscription.StatusCode == 404)
                    {
                        await _systemSubscriptionDataService.ClearOldSubscription();
                        _accountStore.SystemSubscription = null;
                        return false;
                    }
                }
                else
                {
                    if (localsystemSubscription != null)
                    {
                        _accountStore.SystemSubscription = localsystemSubscription;
                        return true;
                    }
                }
            }
            catch
            {
                if (localsystemSubscription != null)
                {
                    _accountStore.SystemSubscription = localsystemSubscription;
                    return true;
                }
            }

            return false;
        }
    }
}
