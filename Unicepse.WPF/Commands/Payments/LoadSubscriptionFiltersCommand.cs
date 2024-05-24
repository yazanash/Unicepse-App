using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.WPF.Commands.Payments
{
    public class LoadSubscriptionFiltersCommand : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayerListItemViewModel _player;


        public LoadSubscriptionFiltersCommand( SubscriptionDataStore subscriptionStore, PlayerListItemViewModel player)
        {
            _subscriptionStore = subscriptionStore;
            _player = player;
        }

        public override async Task ExecuteAsync(object? parameter)
        {

            try
            {

                await _subscriptionStore.GetAll(_player.Player);
            }
            catch (Exception)
            {
            }
            finally
            {
            }
        }
    }
}
