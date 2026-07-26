using Uniceps.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores;

namespace Uniceps.Commands.Payments
{
    public class LoadPaymentsCommand : AsyncCommandBase
    {
        private readonly PaymentDataStore _paymentDataStore;


        public LoadPaymentsCommand(PaymentDataStore paymentDataStore)
        {
            _paymentDataStore = paymentDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                if (parameter is int playerId)
                    await _paymentDataStore.GetAllByPlayer(playerId);
            }
            catch (Exception)
            {
                //_subscriptionListing.ErrorMessage = "Failed to load Players. Please restart the application.";
            }
            finally
            {
                //_subscriptionListing.IsLoading = false;
            }
        }
    }
}
