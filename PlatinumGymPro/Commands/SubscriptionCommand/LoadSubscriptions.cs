using PlatinumGym.Core.Models.Player;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.SubscriptionCommand
{
    public class LoadSubscriptions : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayerMainPageViewModel _subscriptionListing;
        private readonly PlayerListItemViewModel _player;


        public LoadSubscriptions(PlayerMainPageViewModel subscriptionListing, SubscriptionDataStore subscriptionStore, PlayerListItemViewModel player)
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
                _subscriptionListing.ErrorMessage = "Failed to load Players. Please restart the application.";
            }
            finally
            {
                _subscriptionListing.IsLoading = false;
            }
        }
    }
}
