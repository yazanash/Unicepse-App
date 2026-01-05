using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.PaymentsViewModels;

namespace Uniceps.Commands.Payments
{
    public class EditPaymentsCommand : AsyncCommandBase
    {
        private readonly NavigationService<PaymentListViewModel> _navigationService;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private EditPaymentViewModel _editPaymentViewModel;
        public EditPaymentsCommand(NavigationService<PaymentListViewModel> navigationService, PaymentDataStore paymentDataStore, EditPaymentViewModel editPaymentViewModel, PlayersDataStore playersDataStore, SubscriptionDataStore subscriptionDataStore)
        {
            _navigationService = navigationService;
            _paymentDataStore = paymentDataStore;
            _playersDataStore = playersDataStore;
            _editPaymentViewModel = editPaymentViewModel;
            _subscriptionDataStore = subscriptionDataStore;
        }
        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            if (_editPaymentViewModel.PaymentValue > 0)
            {
                _playersDataStore.SelectedPlayer!.Balance -= _paymentDataStore.SelectedPayment!.PaymentValue;

                _playersDataStore.UpdatePlayerBalance(_paymentDataStore.SelectedPayment!.PlayerId, 0 - _paymentDataStore.SelectedPayment!.PaymentValue);
                _paymentDataStore.SelectedPayment.Id = _paymentDataStore.SelectedPayment!.Id;
                _paymentDataStore.SelectedPayment.PayDate = _editPaymentViewModel.PayDate;
                _paymentDataStore.SelectedPayment.PaymentValue = _editPaymentViewModel.PaymentValue;
                _paymentDataStore.SelectedPayment.Des = _editPaymentViewModel.Descriptiones;
                _paymentDataStore.SelectedPayment.PlayerId = _playersDataStore.SelectedPlayer!.Id;
                _paymentDataStore.SelectedPayment.SubscriptionId = _editPaymentViewModel.SelectedSubscription!.Id;
                _paymentDataStore.SelectedPayment.PlayerSyncId = _playersDataStore.SelectedPlayer!.SyncId;
                  _paymentDataStore.SelectedPayment.SubscriptionSyncId = _editPaymentViewModel.SelectedSubscription!.Subscription.SyncId;
                _playersDataStore.SelectedPlayer!.Balance += _paymentDataStore.SelectedPayment.PaymentValue;

                await _paymentDataStore.Update(_paymentDataStore.SelectedPayment!);
                _subscriptionDataStore.UpdateSubscriptionPayments(_paymentDataStore.SelectedPayment.SubscriptionId, _paymentDataStore.SelectedPayment);
                _playersDataStore.UpdatePlayerBalance(_paymentDataStore.SelectedPayment!.PlayerId, _paymentDataStore.SelectedPayment!.PaymentValue);


                _navigationService.ReNavigate();
            }
            else
            {
                MessageBox.Show("لا يمكن ادخال قيمة 0");
            }
        }
    }
}
