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
        private readonly PlayersDataStore _playersDataStore;


        public LoadSubscriptions(ListingViewModelBase subscriptionListing, SubscriptionDataStore subscriptionStore, PlayersDataStore playersDataStore)
        {
            _subscriptionStore = subscriptionStore;
            _subscriptionListing = subscriptionListing;
            _playersDataStore = playersDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _subscriptionListing.ErrorMessage = null;
            _subscriptionListing.IsLoading = true;

            try
            {
                if (_playersDataStore.SelectedPlayer != null)
                    await _subscriptionStore.GetAll(_playersDataStore.SelectedPlayer);
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
