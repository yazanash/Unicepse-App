using PlatinumGym.Core.Models.Payment;
using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Commands.Payments;
using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PaymentsViewModels
{
    public class PaymentListViewModel : ListingViewModelBase
    {
        private readonly PaymentDataStore _paymentDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly ObservableCollection<PaymentListItemViewModel> _paymentListItemViewModels;
        public IEnumerable<PaymentListItemViewModel> PaymentList => _paymentListItemViewModels;
        public ICommand LoadPaymentsCommand { get; }

        public PaymentListViewModel(PaymentDataStore paymentDataStore, PlayersDataStore playersDataStore)
        {
            _paymentDataStore = paymentDataStore;
            _playersDataStore = playersDataStore;
          

            _paymentListItemViewModels = new ObservableCollection<PaymentListItemViewModel>();
            _paymentDataStore.Loaded += _paymentDataStore_Loaded;
            _paymentDataStore.Created += _paymentDataStore_Created;
            _paymentDataStore.Updated += _paymentDataStore_Updated;
            _paymentDataStore.Deleted += _paymentDataStore_Deleted;

            LoadPaymentsCommand = new LoadPaymentsCommand(_playersDataStore, _paymentDataStore);
        }

        private void _paymentDataStore_Deleted(int id)
        {
            PaymentListItemViewModel? itemViewModel = _paymentListItemViewModels.FirstOrDefault(y => y.payment?.Id == id);

            if (itemViewModel != null)
            {
                _paymentListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _paymentDataStore_Updated(PlayerPayment payment)
        {
            PaymentListItemViewModel? subscriptionViewModel =
                   _paymentListItemViewModels.FirstOrDefault(y => y.payment.Id == payment.Id);

            if (subscriptionViewModel != null)
            {
                subscriptionViewModel.Update(payment);
            }
        }

        private void _paymentDataStore_Created(PlayerPayment payment)
        {
            AddPayment(payment);
        }

        private void AddPayment(PlayerPayment payment)
        {
            PaymentListItemViewModel itemViewModel =
             new PaymentListItemViewModel(payment);
            _paymentListItemViewModels.Add(itemViewModel);
        }
        private void _paymentDataStore_Loaded()
        {
            _paymentListItemViewModels.Clear();

            foreach (PlayerPayment payment in _paymentDataStore.Payments)
            {
                AddPayment(payment);
            }
        }
        public override void Dispose()
        {
            _paymentDataStore.Loaded -= _paymentDataStore_Loaded;
            _paymentDataStore.Created -= _paymentDataStore_Created;
            _paymentDataStore.Updated -= _paymentDataStore_Updated;
            _paymentDataStore.Deleted -= _paymentDataStore_Deleted;
            base.Dispose();
        }
        public static PaymentListViewModel LoadViewModel(PaymentDataStore paymentDataStore, PlayersDataStore playersDataStore )
        {
            PaymentListViewModel viewModel = new PaymentListViewModel(paymentDataStore, playersDataStore);
            viewModel.LoadPaymentsCommand.Execute(null);
            return viewModel;
        }
    }
}
