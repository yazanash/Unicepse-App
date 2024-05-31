using Unicepse.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;

namespace Unicepse.Commands.Payments
{
    public class LoadPaymentsCommand : AsyncCommandBase
    {
        private readonly PlayersDataStore _playersDataStore;
        private readonly PaymentDataStore _paymentDataStore;


        public LoadPaymentsCommand(PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore)
        {
            _playersDataStore = playersDataStore;
            _paymentDataStore = paymentDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            //try
            //{

            await _paymentDataStore.GetPlayerPayments(_playersDataStore.SelectedPlayer!.Player);
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
