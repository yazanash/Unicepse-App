using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Payment;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.Commands.Payments
{
    public class SubmitPaymentCommand : AsyncCommandBase
    {
        private readonly PaymentDataStore _paymentDataStore;
        private AddPaymentViewModel _addPaymentViewModel;
        public SubmitPaymentCommand(PaymentDataStore paymentDataStore, AddPaymentViewModel addPaymentViewModel)
        {
            _paymentDataStore = paymentDataStore;
            _addPaymentViewModel = addPaymentViewModel;
            _addPaymentViewModel.PropertyChanged += _addPaymentViewModel_PropertyChanged;
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
            try
            {
                DateTime payd = Convert.ToDateTime(_addPaymentViewModel.PayDate.ToShortDateString());
                if (_addPaymentViewModel.PaymentValue > 0)
                {
                    PlayerPayment payment = new PlayerPayment()
                    {
                        PayDate = _addPaymentViewModel.PayDate,
                        PaymentValue = _addPaymentViewModel.PaymentValue,
                        Des = _addPaymentViewModel.Descriptiones,
                        PlayerId = _addPaymentViewModel.PlayerId,
                        SubscriptionId = _addPaymentViewModel.SelectedSubscription!.Id,
                        SubscriptionSyncId = _addPaymentViewModel.SelectedSubscription!.Subscription.SyncId
                    };
                   
                    int sportDays = _addPaymentViewModel.SelectedSubscription!.DaysCount;
                    double dayPrice = _addPaymentViewModel.SelectedSubscription!.PriceAfterOffer / sportDays;
                    int daysCount = Convert.ToInt32(payment.PaymentValue / dayPrice);
                    if (_addPaymentViewModel.IsEditMode)
                    {
                        payment.Id = _addPaymentViewModel.Id;
                        await _paymentDataStore.Update(payment);
                    }
                    else
                        await _paymentDataStore.Add(payment);

                    _addPaymentViewModel.OnRequestClose();

                }
                else
                {
                    MessageBox.Show("لا يمكن ادخال قيمة 0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }
    }
}
