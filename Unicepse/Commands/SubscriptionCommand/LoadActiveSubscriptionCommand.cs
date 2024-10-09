using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels;

namespace Unicepse.Commands.SubscriptionCommand
{
    public class LoadActiveSubscriptionCommand : AsyncCommandBase
     {
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly ListingViewModelBase _subscriptionListing;


        public LoadActiveSubscriptionCommand(SubscriptionDataStore subscriptionStore, ListingViewModelBase subscriptionListing)
        {
            _subscriptionStore = subscriptionStore;
            _subscriptionListing = subscriptionListing;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _subscriptionListing.ErrorMessage = null;
            _subscriptionListing.IsLoading = true;

            try
            {

                await _subscriptionStore.GetAllActive();
            }
            catch (Exception)
            {
                _subscriptionListing.ErrorMessage = "خطأ في تحميل الاشتراكات يرجى اعادة تشغيل البرنامج";
            }
            finally
            {
                _subscriptionListing.IsLoading = false;
            }
        }
    }
}
