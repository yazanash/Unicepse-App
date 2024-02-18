using PlatinumGym.Core.Models.Payment;
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
        private readonly NavigationService<PlayerMainPageViewModel> _navigationService;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private AddPaymentViewModel _addPaymentViewModel;
        public SubmitPaymentCommand(NavigationService<PlayerMainPageViewModel> navigationService,PaymentDataStore paymentDataStore, AddPaymentViewModel addPaymentViewModel, PlayersDataStore playersDataStore)
        {
            _navigationService = navigationService;
            _paymentDataStore = paymentDataStore;
            _playersDataStore = playersDataStore;
            _addPaymentViewModel = addPaymentViewModel;
        }
        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {

            PlayerPayment payment = new PlayerPayment()
            {
                PayDate = _addPaymentViewModel.PayDate,
                PaymentValue = _addPaymentViewModel.PaymentValue,
                Des = _addPaymentViewModel.Descriptiones,
                Subscription = _addPaymentViewModel.SelectedSubscription!.Subscription,
                Player = _addPaymentViewModel.SelectedSubscription!.Subscription.Player

            };

            await _paymentDataStore.Add(payment);
            MessageBox.Show("payment added successfully");
            _navigationService.Navigate();
        }
    }
}
