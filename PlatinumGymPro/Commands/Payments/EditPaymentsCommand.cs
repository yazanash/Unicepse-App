using PlatinumGym.Core.Models.Payment;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PaymentsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.Payments
{
    public class EditPaymentsCommand :AsyncCommandBase
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
            _paymentDataStore.SelectedPayment!.Subscription!.IsPaid = false;
            _paymentDataStore.SelectedPayment.Subscription.PaidValue -= _paymentDataStore.SelectedPayment.PaymentValue;
            await _subscriptionDataStore.Update(_paymentDataStore.SelectedPayment.Subscription);
            PlayerPayment payment = new PlayerPayment()
            {
                Id = _paymentDataStore.SelectedPayment!.Id,
                PayDate = _editPaymentViewModel.PayDate,
                PaymentValue = _editPaymentViewModel.PaymentValue,
                Des = _editPaymentViewModel.Descriptiones,
                Subscription = _editPaymentViewModel.SelectedSubscription!.Subscription,
                Player = _editPaymentViewModel.SelectedSubscription!.Subscription.Player,

            };
            _subscriptionDataStore.SelectedSubscription!.PaidValue += payment.PaymentValue;
            if (_subscriptionDataStore.SelectedSubscription!.PaidValue == _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer)
                _subscriptionDataStore.SelectedSubscription.IsPaid = true;

            await _paymentDataStore.Update(payment);
            await _subscriptionDataStore.Update(_subscriptionDataStore.SelectedSubscription);
            MessageBox.Show("payment updated successfully");
            _navigationService.ReNavigate();
        }
    }
}
