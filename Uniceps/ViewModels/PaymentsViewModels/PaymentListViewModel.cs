using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Payments;
using Uniceps.Commands.Player;
using Uniceps.Core.Models.Payment;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.Views.PaymentViews;

namespace Uniceps.ViewModels.PaymentsViewModels
{
    public class PaymentListViewModel : ListingViewModelBase
    {
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly ObservableCollection<PaymentListItemViewModel> _paymentListItemViewModels;
        public IEnumerable<PaymentListItemViewModel> PaymentList => _paymentListItemViewModels;
        public CollectionViewSource GroupedTasks { get; set; }
        public bool HasData => _paymentListItemViewModels.Count > 0;
        public ICommand LoadPaymentsCommand { get; }
        public ICommand AddPaymentsCommand => new RelayCommand(ExecuteAddPayment);

        public ICommand UpdatePaymentsCommand => new RelayCommand<PaymentListItemViewModel>(ExecuteUpdatePayment);

        public ICommand DeletePaymentsCommand => new AsyncRelayCommand<PaymentListItemViewModel>(ExecuteDeletePayment);

        private async Task ExecuteDeletePayment(PaymentListItemViewModel? model)
        {
            if (model != null)
            {
                if (MessageBox.Show("سيتم حذف هذا الدفعة , هل انت متاكد", "تنبيه", MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    await _paymentDataStore.Delete(model.payment.Id);
                    MessageBox.Show("تم حذف الدفعة بنجاح");
                }
            }
           
        }

        private void ExecuteUpdatePayment(PaymentListItemViewModel paymentListItemViewModel)
        {
            AddPaymentViewModel paymentViewModel = new AddPaymentViewModel(paymentListItemViewModel.payment, _paymentDataStore, _subscriptionDataStore);
            PaymentDetailWindowView paymentDetailWindowView = new PaymentDetailWindowView();
            paymentDetailWindowView.DataContext = paymentViewModel;
            paymentDetailWindowView.ShowDialog();
        }

        private void ExecuteAddPayment()
        {
            AddPaymentViewModel paymentViewModel = new AddPaymentViewModel(PlayerId, _paymentDataStore, _subscriptionDataStore);
            PaymentDetailWindowView paymentDetailWindowView = new PaymentDetailWindowView();
            paymentDetailWindowView.DataContext = paymentViewModel;
            paymentDetailWindowView.ShowDialog();
        }

        public int PlayerId;

        public PaymentListViewModel(int playerId,PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore)
        {
            PlayerId = playerId;
            _paymentDataStore = paymentDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _paymentListItemViewModels = new ObservableCollection<PaymentListItemViewModel>();
            GroupedTasks = new CollectionViewSource { Source = _paymentListItemViewModels };
            _paymentDataStore.Loaded += _paymentDataStore_Loaded;
            _paymentDataStore.Created += _paymentDataStore_Created;
            _paymentDataStore.Updated += _paymentDataStore_Updated;
            _paymentDataStore.Deleted += _paymentDataStore_Deleted;
            LoadPaymentsCommand = new LoadPaymentsCommand(_paymentDataStore);
            LoadPaymentsCommand.Execute(PlayerId);

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
             new PaymentListItemViewModel(payment);
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
      
    }
}
