using Unicepse.Core.Models.Payment;
using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.PaymentsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.WPF.navigation;

namespace Unicepse.WPF.Commands.Payments
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
            _subscriptionDataStore.SelectedSubscription!.IsPaid = false;
            _subscriptionDataStore.SelectedSubscription!.PaidValue -= _paymentDataStore.SelectedPayment!.PaymentValue;
            _playersDataStore.SelectedPlayer!.Player.Balance -= _paymentDataStore.SelectedPayment.PaymentValue;


            _paymentDataStore.SelectedPayment.Id = _paymentDataStore.SelectedPayment!.Id;
            _paymentDataStore.SelectedPayment.PayDate = _editPaymentViewModel.PayDate;
            _paymentDataStore.SelectedPayment.PaymentValue = _editPaymentViewModel.PaymentValue;
            _paymentDataStore.SelectedPayment.Des = _editPaymentViewModel.Descriptiones;
            _paymentDataStore.SelectedPayment.Subscription = _subscriptionDataStore.SelectedSubscription!;
            _paymentDataStore.SelectedPayment.Player = _playersDataStore.SelectedPlayer!.Player;


            _subscriptionDataStore.SelectedSubscription!.PaidValue += _paymentDataStore.SelectedPayment.PaymentValue;
            _playersDataStore.SelectedPlayer!.Player.Balance += _paymentDataStore.SelectedPayment.PaymentValue;
            if (_subscriptionDataStore.SelectedSubscription!.PaidValue == _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer)
                _subscriptionDataStore.SelectedSubscription.IsPaid = true;

            await _paymentDataStore.Update(_paymentDataStore.SelectedPayment!);
            await _subscriptionDataStore.Update(_subscriptionDataStore.SelectedSubscription);
            await _playersDataStore.UpdatePlayer(_playersDataStore.SelectedPlayer!.Player);
            _navigationService.ReNavigate();
        }
    }
}
