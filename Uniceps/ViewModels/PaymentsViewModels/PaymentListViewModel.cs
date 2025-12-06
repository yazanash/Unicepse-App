using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Payments;
using Uniceps.Commands.Player;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.ViewModels.PaymentsViewModels
{
    public class PaymentListViewModel : ListingViewModelBase
    {
        private readonly PaymentDataStore _paymentDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly ObservableCollection<PaymentListItemViewModel> _paymentListItemViewModels;
        public IEnumerable<PaymentListItemViewModel> PaymentList => _paymentListItemViewModels;
        public CollectionViewSource GroupedTasks { get; set; }
        public bool HasData => _paymentListItemViewModels.Count > 0;
        public ICommand LoadPaymentsCommand { get; }
        public ICommand AddPaymentsCommand { get; }

        public PaymentListViewModel(PaymentDataStore paymentDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore)
        {
            _paymentDataStore = paymentDataStore;
            _playersDataStore = playersDataStore;
            _navigationStore = navigationStore;
            _subscriptionDataStore = subscriptionDataStore;

            AddPaymentsCommand = new NavaigateCommand<AddPaymentViewModel>(new NavigationService<AddPaymentViewModel>(navigationStore, () => LoadAddPaymentViewModel(_paymentDataStore, _subscriptionDataStore, _playersDataStore, _navigationStore, this)));
            _paymentListItemViewModels = new ObservableCollection<PaymentListItemViewModel>();
            GroupedTasks = new CollectionViewSource { Source = _paymentListItemViewModels };
            _paymentDataStore.Loaded += _paymentDataStore_Loaded;
            _paymentDataStore.Created += _paymentDataStore_Created;
            _paymentDataStore.Updated += _paymentDataStore_Updated;
            _paymentDataStore.Deleted += _paymentDataStore_Deleted;

            LoadPaymentsCommand = new LoadPaymentsCommand(_playersDataStore, _paymentDataStore);
        }

        public PaymentListItemViewModel? SelectedPayment
        {
            get
            {
                return PaymentList
                    .FirstOrDefault(y => y?.payment == _paymentDataStore.SelectedPayment);
            }
            set
            {
                _paymentDataStore.SelectedPayment = value?.payment;
                OnPropertyChanged(nameof(SelectedPayment));
            }
        }




        private void _paymentDataStore_Deleted(int id)
        {
            PaymentListItemViewModel? itemViewModel = _paymentListItemViewModels.FirstOrDefault(y => y.payment?.Id == id);

            if (itemViewModel != null)
            {
                _paymentListItemViewModels.Remove(itemViewModel);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _paymentDataStore_Updated(PlayerPayment payment)
        {
            PaymentListItemViewModel? subscriptionViewModel =
                   _paymentListItemViewModels.FirstOrDefault(y => y.payment.Id == payment.Id);

            if (subscriptionViewModel != null)
            {
                subscriptionViewModel.Update(payment);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _paymentDataStore_Created(PlayerPayment payment)
        {
            LoadData();
            OnPropertyChanged(nameof(HasData));
        }

        private void AddPayment(PlayerPayment payment)
        {
            PaymentListItemViewModel itemViewModel =
             new PaymentListItemViewModel(payment, _paymentDataStore, _subscriptionDataStore, _playersDataStore, _navigationStore, this);
            _paymentListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _paymentListItemViewModels.Count();
            OnPropertyChanged(nameof(HasData));

        }
        private void _paymentDataStore_Loaded()
        {
            LoadData();
        }
        void LoadData()
        {
            _paymentListItemViewModels.Clear();

            foreach (PlayerPayment payment in _paymentDataStore.Payments.OrderByDescending(x => x.PayDate))
            {
                AddPayment(payment);
            }
            GroupedTasks.Source = _paymentListItemViewModels;
            OnPropertyChanged(nameof(GroupedTasks));
        }
        public override void Dispose()
        {
            _paymentDataStore.Loaded -= _paymentDataStore_Loaded;
            _paymentDataStore.Created -= _paymentDataStore_Created;
            _paymentDataStore.Updated -= _paymentDataStore_Updated;
            _paymentDataStore.Deleted -= _paymentDataStore_Deleted;
            base.Dispose();
        }
        private AddPaymentViewModel LoadAddPaymentViewModel(PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, NavigationStore navigatorStore, PaymentListViewModel paymentListViewModel)
        {
            return AddPaymentViewModel.LoadViewModel(paymentDataStore, subscriptionDataStore, playersDataStore, navigatorStore, paymentListViewModel);
        }
        public static PaymentListViewModel LoadViewModel(PaymentDataStore paymentDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore)
        {
            PaymentListViewModel viewModel = new PaymentListViewModel(paymentDataStore, playersDataStore, navigationStore, subscriptionDataStore);
            viewModel.LoadPaymentsCommand.Execute(null);
            return viewModel;
        }
    }
}
