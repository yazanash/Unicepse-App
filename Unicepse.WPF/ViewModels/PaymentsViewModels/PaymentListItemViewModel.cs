using Unicepse.Core.Models.Payment;
using Unicepse.WPF.Commands;
using Unicepse.WPF.Commands.Payments;
using Unicepse.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.WPF.navigation.Stores;
using Unicepse.WPF.utlis.common;
using Unicepse.WPF.Commands.Player;
using Unicepse.WPF.navigation;
namespace Unicepse.WPF.ViewModels.PaymentsViewModels
{
    public class PaymentListItemViewModel : ViewModelBase
    {
        private readonly PaymentDataStore? _paymentDataStore;
        private readonly SubscriptionDataStore? _subscriptionDataStore;
        private readonly PlayersDataStore? _playersDataStore;
        private readonly NavigationStore? _navigatorStore;
        private readonly PaymentListViewModel? _paymentListViewModel;

        public PlayerPayment payment;
        public int Id => payment.Id;
        public int SubscriptionId => payment.Subscription!.Id;
        public string? Description => payment.Des;
        public double Value => payment.PaymentValue;
        public string? Date => payment.PayDate.ToShortDateString();
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
