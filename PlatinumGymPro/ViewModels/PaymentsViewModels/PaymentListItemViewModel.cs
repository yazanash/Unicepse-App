using PlatinumGym.Core.Models.Payment;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.Payments;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PaymentsViewModels
{
    public class PaymentListItemViewModel : ViewModelBase
    {
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationStore _navigatorStore;
        private readonly PaymentListViewModel _paymentListViewModel;

        public PlayerPayment payment;
        public int Id => payment.Id;
        public int SubscriptionId => payment.Subscription!.Id;
        public string? Description => payment.Des;
        public double Value => payment.PaymentValue;
        public DateTime Date => payment.PayDate;
        public PaymentListItemViewModel(PlayerPayment payment, PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, NavigationStore navigatorStore, PaymentListViewModel paymentListViewModel)
        {
            this.payment = payment;
            _paymentDataStore = paymentDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _playersDataStore = playersDataStore;
            _navigatorStore = navigatorStore;
            _paymentListViewModel = paymentListViewModel;
            EditCommand = new NavaigateCommand<EditPaymentViewModel>(new NavigationService<EditPaymentViewModel>(_navigatorStore, () => LoadEditPaymentViewModel( _paymentDataStore,  _subscriptionDataStore,  _playersDataStore,  _navigatorStore,  _paymentListViewModel)));
            DeleteCommand = new DeletePaymentCommand(_paymentDataStore, _playersDataStore, _subscriptionDataStore);
        }

        public PaymentListItemViewModel(PlayerPayment payment)
        {
            this.payment = payment;
            
        }
        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }
        public void Update(PlayerPayment payment)
        {
            this.payment = payment;
        }


        private EditPaymentViewModel LoadEditPaymentViewModel(PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, NavigationStore navigatorStore, PaymentListViewModel paymentListViewModel)
        {
            return EditPaymentViewModel.LoadViewModel(paymentDataStore, subscriptionDataStore, playersDataStore, navigatorStore, paymentListViewModel);
        }
    }
}
