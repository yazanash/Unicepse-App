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
using Uniceps.navigation.DialogService;
using Uniceps.navigation.Extensions;
using Uniceps.Stores;
using Uniceps.Views.PaymentViews;

namespace Uniceps.ViewModels.PaymentsViewModels
{
    public class PaymentListViewModel : ListingViewModelBase,IQueryAttributable
    {
        private readonly PaymentDataStore _paymentDataStore;
        private readonly IDialogService _dialogService;

        private readonly ObservableCollection<PaymentListItemViewModel> _paymentListItemViewModels;
        public IEnumerable<PaymentListItemViewModel> PaymentList => _paymentListItemViewModels;

        public CollectionViewSource GroupedTasks { get; set; }

        public int PlayerId;
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
            var navParams = new Dictionary<string, object>
            {
                { "Payment", paymentListItemViewModel.payment },
            };
            _dialogService.ShowDialog<PaymentDetailWindowView, AddPaymentViewModel>(navParams);
        }

        private void ExecuteAddPayment()
        {
            var navParams = new Dictionary<string, object>
            {
                { "PlayerId", PlayerId },
            };
            _dialogService.ShowDialog<PaymentDetailWindowView, AddPaymentViewModel>(navParams);
        }


        public PaymentListViewModel( PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, IDialogService dialogService)
        {
            _paymentDataStore = paymentDataStore;
            _dialogService = dialogService;

            _paymentListItemViewModels = new ObservableCollection<PaymentListItemViewModel>();
            GroupedTasks = new CollectionViewSource { Source = _paymentListItemViewModels };
            _paymentDataStore.Loaded += _paymentDataStore_Loaded;
            _paymentDataStore.Created += _paymentDataStore_Created;
            _paymentDataStore.Updated += _paymentDataStore_Updated;
            _paymentDataStore.Deleted += _paymentDataStore_Deleted;
            LoadPaymentsCommand = new LoadPaymentsCommand(_paymentDataStore);
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<int>("PlayerId", out var playerId))
            {
                PlayerId = playerId;
                LoadPaymentsCommand.Execute(PlayerId);
            }

        }
    }
}
