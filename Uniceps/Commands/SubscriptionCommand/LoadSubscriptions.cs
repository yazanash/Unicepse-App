using Uniceps.Core.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.Commands;
using Uniceps.ViewModels;
using Uniceps.Stores;

namespace Uniceps.Commands.SubscriptionCommand
{
    public class LoadSubscriptions : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly ListingViewModelBase _subscriptionListing;


        public LoadSubscriptions(ListingViewModelBase subscriptionListing, SubscriptionDataStore subscriptionStore)
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
                if(parameter is int playerId)
                    await _subscriptionStore.GetAllByPlayer(playerId);
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
