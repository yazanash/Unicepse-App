using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;

namespace Unicepse.Commands.Payments
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
            _subscriptionDataStore.SelectedSubscription = _paymentDataStore.SelectedPayment!.Subscription;
            _subscriptionDataStore.SelectedSubscription!.IsPaid = false;
            _subscriptionDataStore.SelectedSubscription.PaidValue -= _paymentDataStore.SelectedPayment!.PaymentValue;
            await _paymentDataStore.Delete(_paymentDataStore.SelectedPayment!.Id);
            await _subscriptionDataStore.Update(_subscriptionDataStore.SelectedSubscription!);
            MessageBox.Show("payment deleted successfully");
        }
    }
}
