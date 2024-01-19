using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.Payments
{
    public class LoadPaymentsCommand : AsyncCommandBase
    {
        private readonly PlayerListItemViewModel _player;
        private readonly PaymentDataStore _paymentDataStore;


        public LoadPaymentsCommand(PlayerListItemViewModel player, PaymentDataStore paymentDataStore)
        {
            _player = player;
            _paymentDataStore = paymentDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            //try
            //{

                await _paymentDataStore.GetPlayerPayments(_player.Player);
            //}
            //catch (Exception)
            //{
            //    //_subscriptionListing.ErrorMessage = "Failed to load Players. Please restart the application.";
            //}
            //finally
            //{
            //    //_subscriptionListing.IsLoading = false;
            //}
        }
    }
}
