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
            if (MessageBox.Show("سيتم حذف هذا الدفعة , هل انت متاكد", "تنبيه", MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _subscriptionDataStore.SelectedSubscription = _paymentDataStore.SelectedPayment!.Subscription;
                if (_subscriptionDataStore.SelectedSubscription!.Player == null)
                {
                    _subscriptionDataStore.SelectedSubscription!.Player = new Core.Models.Player.Player() { Id = _playersDataStore.SelectedPlayer!.Id };
                }
                _subscriptionDataStore.SelectedSubscription!.IsPaid = false;
                _subscriptionDataStore.SelectedSubscription.PaidValue -= _paymentDataStore.SelectedPayment!.PaymentValue;
                _playersDataStore.SelectedPlayer!.Player.Balance -= _paymentDataStore.SelectedPayment!.PaymentValue;

                await _playersDataStore.UpdatePlayer(_playersDataStore.SelectedPlayer!.Player);
                await _paymentDataStore.Delete(_paymentDataStore.SelectedPayment!);
                await _subscriptionDataStore.Update(_subscriptionDataStore.SelectedSubscription!);
                MessageBox.Show("تم حذف الدفعة بنجاح");
            }
              
        }
    }
}
