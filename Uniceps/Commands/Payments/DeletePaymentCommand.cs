using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Stores;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.Commands.Payments
{
    public class DeletePaymentCommand : AsyncCommandBase
    {
        private readonly PlayersDataStore _playersDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        public DeletePaymentCommand(PaymentDataStore paymentDataStore, PlayersDataStore playersDataStore, SubscriptionDataStore subscriptionDataStore)
        {
            _paymentDataStore = paymentDataStore;
            _playersDataStore = playersDataStore;
            _subscriptionDataStore = subscriptionDataStore;
        }
        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            if (MessageBox.Show("سيتم حذف هذا الدفعة , هل انت متاكد", "تنبيه", MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _playersDataStore.UpdatePlayerBalance(_paymentDataStore.SelectedPayment!.PlayerId, 0 - _paymentDataStore.SelectedPayment!.PaymentValue);
                _subscriptionDataStore.RemoveSubscriptionPayments(_paymentDataStore.SelectedPayment!.SubscriptionId,_paymentDataStore.SelectedPayment!.Id);
                await _paymentDataStore.Delete(_paymentDataStore.SelectedPayment!.Id);
                MessageBox.Show("تم حذف الدفعة بنجاح");
            }

        }
    }
}
