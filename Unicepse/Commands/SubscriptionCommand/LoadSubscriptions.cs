using Unicepse.Core.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.ViewModels.PlayersViewModels;

namespace Unicepse.Commands.SubscriptionCommand
{
    public class LoadSubscriptions : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly ListingViewModelBase _subscriptionListing;
        private readonly PlayerListItemViewModel _player;


        public LoadSubscriptions(ListingViewModelBase subscriptionListing, SubscriptionDataStore subscriptionStore, PlayerListItemViewModel player)
        {
            _subscriptionStore = subscriptionStore;
            _subscriptionListing = subscriptionListing;
            _player = player;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _subscriptionListing.ErrorMessage = null;
            _subscriptionListing.IsLoading = true;

            try
            {

                await _subscriptionStore.GetAll(_player.Player);
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
