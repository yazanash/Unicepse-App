﻿using PlatinumGym.Core.Models.Payment;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels;
using PlatinumGymPro.ViewModels.PaymentsViewModels;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.Payments
{
    public class SubmitPaymentCommand : AsyncCommandBase
    {
        private readonly NavigationService<PaymentListViewModel> _navigationService;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private AddPaymentViewModel _addPaymentViewModel;
        public SubmitPaymentCommand(NavigationService<PaymentListViewModel> navigationService, PaymentDataStore paymentDataStore, AddPaymentViewModel addPaymentViewModel, PlayersDataStore playersDataStore, SubscriptionDataStore subscriptionDataStore)
        {
            _navigationService = navigationService;
            _paymentDataStore = paymentDataStore;
            _playersDataStore = playersDataStore;
            _addPaymentViewModel = addPaymentViewModel;
            _addPaymentViewModel.PropertyChanged += _addPaymentViewModel_PropertyChanged;
            _subscriptionDataStore = subscriptionDataStore;
        }

        private void _addPaymentViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_addPaymentViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

       

        public override bool CanExecute(object? parameter)
        {
            return _addPaymentViewModel.CanSubmit && _addPaymentViewModel.SelectedSubscription!=null && base.CanExecute(null);
        }
        public override async Task ExecuteAsync(object? parameter)
        {

            PlayerPayment payment = new PlayerPayment()
            {
                PayDate = _addPaymentViewModel.PayDate,
                PaymentValue = _addPaymentViewModel.PaymentValue,
                Des = _addPaymentViewModel.Descriptiones,
                Subscription = _addPaymentViewModel.SelectedSubscription!.Subscription,
                Player = _addPaymentViewModel.SelectedSubscription!.Subscription.Player,
                
            };
            _subscriptionDataStore.SelectedSubscription!.PaidValue += payment.PaymentValue;
            int sportDays = _subscriptionDataStore.SelectedSubscription!.DaysCount;
            double dayPrice = _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer / sportDays;
            int daysCount = Convert.ToInt32(payment.PaymentValue / dayPrice);
            payment.From = _subscriptionDataStore.SelectedSubscription!.LastPaid;
            _subscriptionDataStore.SelectedSubscription!.LastPaid = _subscriptionDataStore.SelectedSubscription!.LastPaid.AddDays(daysCount);
            payment.To = _subscriptionDataStore.SelectedSubscription!.LastPaid;
            if (_subscriptionDataStore.SelectedSubscription!.PaidValue == _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer)
                _subscriptionDataStore.SelectedSubscription.IsPaid= true;

            await _paymentDataStore.Add(payment);
            await _subscriptionDataStore.Update(_subscriptionDataStore.SelectedSubscription);
            MessageBox.Show("payment added successfully");
            _navigationService.ReNavigate();
        }
    }
}
