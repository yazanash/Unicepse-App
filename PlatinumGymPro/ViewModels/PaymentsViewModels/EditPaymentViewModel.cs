using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Commands.Payments;
using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PaymentsViewModels
{
    public class EditPaymentViewModel : ListingViewModelBase
    {
        private readonly PaymentDataStore _paymentDataStore;
        private readonly NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PaymentListViewModel _paymentListViewModel;
        private readonly ObservableCollection<SubscriptionCardViewModel> _subscriptionListViewModel;
        public IEnumerable<SubscriptionCardViewModel> SubscriptionList => _subscriptionListViewModel;
        public EditPaymentViewModel(PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, NavigationStore navigatorStore, PaymentListViewModel paymentListViewModel)
        {
            _paymentDataStore = paymentDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _playersDataStore = playersDataStore;
            _navigatorStore = navigatorStore;
            _paymentListViewModel = paymentListViewModel;
            _subscriptionListViewModel = new ObservableCollection<SubscriptionCardViewModel>();
            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionDataStore, _playersDataStore.SelectedPlayer!);
            _subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            SubmitCommand = new EditPaymentsCommand(new NavigationService<PaymentListViewModel>(_navigatorStore, () => _paymentListViewModel), _paymentDataStore, this, _playersDataStore, _subscriptionDataStore);

            PaymentValue = _paymentDataStore.SelectedPayment!.PaymentValue;
            Descriptiones = _paymentDataStore.SelectedPayment!.Des;
            PayDate = _paymentDataStore.SelectedPayment!.PayDate;


        }

        private void _subscriptionDataStore_Loaded()
        {
            _subscriptionListViewModel.Clear();

            foreach (Subscription subscription in _subscriptionDataStore.Subscriptions.Where(x => !x.IsPaid))
            {
                AddSubscriptiont(subscription);
            }
            if (!_subscriptionListViewModel.Any(x=>x.Id==_paymentDataStore.SelectedPayment!.Subscription!.Id))
            {
                AddSubscriptiont(_paymentDataStore.SelectedPayment!.Subscription!);
            }
            SelectedSubscription = SubscriptionList.FirstOrDefault(x => x.Id == _paymentDataStore.SelectedPayment!.Subscription!.Id);
        }

        ICommand LoadSubscriptionCommand;

        public ICommand SubmitCommand { get; }
        //ICommand CancelCommand;
        #region Properties
        private double _paymentValue;
        public double PaymentValue
        {
            get { return _paymentValue; }
            set { _paymentValue = value; OnPropertyChanged(nameof(PaymentValue)); }
        }
        private string? _descriptiones;
        public string? Descriptiones
        {
            get { return _descriptiones; }
            set { _descriptiones = value; OnPropertyChanged(nameof(Descriptiones)); }
        }
        private DateTime _payDate = DateTime.Now;
        public DateTime PayDate
        {
            get { return _payDate; }
            set { _payDate = value; OnPropertyChanged(nameof(PayDate)); }
        }
        #endregion
        public SubscriptionCardViewModel? SelectedSubscription
        {
            get
            {
                return SubscriptionList
                    .FirstOrDefault(y => y?.Subscription == _subscriptionDataStore.SelectedSubscription);
            }
            set
            {
                _subscriptionDataStore.SelectedSubscription = value?.Subscription;
                OnPropertyChanged(nameof(SelectedSubscription));
            }
        }
        private void AddSubscriptiont(Subscription subscription)
        {
            SubscriptionCardViewModel itemViewModel =
                new SubscriptionCardViewModel(subscription);
            _subscriptionListViewModel.Add(itemViewModel);
        }
        public static EditPaymentViewModel LoadViewModel(PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, PaymentListViewModel paymentListViewModel)
        {
            EditPaymentViewModel viewModel = new(paymentDataStore, subscriptionDataStore, playersDataStore, navigationStore, paymentListViewModel);

            viewModel.LoadSubscriptionCommand.Execute(null);

            return viewModel;
        }
    }
}
