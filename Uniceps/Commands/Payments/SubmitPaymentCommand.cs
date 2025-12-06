using Uniceps.ViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.Core.Models.Payment;

namespace Uniceps.Commands.Payments
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
            return _addPaymentViewModel.CanSubmit && _addPaymentViewModel.SelectedSubscription != null && base.CanExecute(null);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            DateTime payd = Convert.ToDateTime(_addPaymentViewModel.PayDate.ToShortDateString());
            if (_addPaymentViewModel.PaymentValue > 0)
            {
                PlayerPayment payment = new PlayerPayment()
                {
                    PayDate = _addPaymentViewModel.PayDate,
                    PaymentValue = _addPaymentViewModel.PaymentValue,
                    Des = _addPaymentViewModel.Descriptiones,
                    PlayerId = _playersDataStore.SelectedPlayer!.Id,
                    SubscriptionId = _addPaymentViewModel.SelectedSubscription!.Id
                };
                _playersDataStore.SelectedPlayer!.IsSubscribed = true;
               
                int sportDays = _subscriptionDataStore.SelectedSubscription!.DaysCount;
                double dayPrice = _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer / sportDays;
                int daysCount = Convert.ToInt32(payment.PaymentValue / dayPrice);

                await _paymentDataStore.Add(payment);
                await _subscriptionDataStore.Update(_subscriptionDataStore.SelectedSubscription);
                _playersDataStore.UpdatePlayerBalance(payment.PlayerId, payment.PaymentValue);
                _navigationService.ReNavigate();
            }
            else
            {
                MessageBox.Show("لا يمكن ادخال قيمة 0");
            }
        }
    }
}
