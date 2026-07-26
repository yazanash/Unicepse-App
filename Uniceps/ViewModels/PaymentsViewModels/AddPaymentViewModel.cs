using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Uniceps.Commands.Payments;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Subscription;
using Uniceps.Stores;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.ViewModels.PaymentsViewModels
{
    public class AddPaymentViewModel : ListingViewModelBase, INotifyDataErrorInfo
    {
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly ObservableCollection<SubscriptionCardViewModel> _subscriptionListViewModel;
        public IEnumerable<SubscriptionCardViewModel> SubscriptionList => _subscriptionListViewModel;
        public int PlayerId;
        public int Id;
        public int SubscriptionId;
        public bool IsEditMode;

        public Action? RequestClose;
        public void OnRequestClose()
        {
            RequestClose?.Invoke();
        }
        public AddPaymentViewModel(int playerId, PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore)
        {
            PlayerId = playerId;
            _paymentDataStore = paymentDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _subscriptionListViewModel = new ObservableCollection<SubscriptionCardViewModel>();
            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionDataStore);
            _subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            SubmitCommand = new SubmitPaymentCommand(_paymentDataStore, this);
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            LoadSubscriptionCommand.Execute(PlayerId);
        }
        public AddPaymentViewModel(PlayerPayment playerPayment, PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore)
        {
            PlayerId = playerPayment.PlayerId;
            SubscriptionId = playerPayment.SubscriptionId;
            Id = playerPayment.Id;
            PaymentValue = playerPayment.PaymentValue;
            PayDate = playerPayment.PayDate;
            Descriptiones = playerPayment.Des;
            _paymentDataStore = paymentDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _subscriptionListViewModel = new ObservableCollection<SubscriptionCardViewModel>();
            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionDataStore);
            _subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            SubmitCommand = new SubmitPaymentCommand(_paymentDataStore, this);
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            LoadSubscriptionCommand.Execute(PlayerId);
        }

        private void _subscriptionDataStore_Loaded()
        {
            _subscriptionListViewModel.Clear();

            foreach (Subscription subscription in _subscriptionDataStore.Subscriptions.Where(x => x.TotalPaid < x.PriceAfterOffer))
            {
                AddSubscriptiont(subscription);
            }
            if (SubscriptionId > 0)
            {
                SelectedSubscription = _subscriptionListViewModel.FirstOrDefault(x => x.Id == SubscriptionId);
            }
        }

        ICommand LoadSubscriptionCommand;

        public ICommand SubmitCommand { get; }
        //ICommand CancelCommand;
        #region Properties
        private double _paymentValue;
        public double PaymentValue
        {
            get { return _paymentValue; }
            set
            {
                _paymentValue = value;
                OnPropertyChanged(nameof(PaymentValue));
                ClearError(nameof(PaymentValue));
                if (SelectedSubscription != null)
                {
                    if (PaymentValue < 0)
                    {
                        AddError("لايمكن الدفع بقيمة اقل من 0", nameof(PaymentValue));
                        OnErrorChanged(nameof(PaymentValue));
                    }
                    if (PaymentValue > SelectedSubscription.Subscription.PriceAfterOffer - SelectedSubscription.Subscription.TotalPaid)
                    {
                        AddError("لايمكن الدفع بقيمة اكثر من المستحق", nameof(PaymentValue));
                        OnErrorChanged(nameof(PaymentValue));
                    }
                }

                else
                {
                    AddError("يجب اختيار الاشتراك اولا", nameof(PaymentValue));
                    OnErrorChanged(nameof(PaymentValue));
                }

            }
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
            set
            {
                _payDate = value;
                OnPropertyChanged(nameof(PayDate));
                ClearError(nameof(PayDate));
                if (SelectedSubscription != null)
                {
                    if (PayDate < Convert.ToDateTime(SelectedSubscription!.RollDate))
                    {
                        AddError("لا يمكن ان يكون تاريخ الدفعة اصغر من تاريخ الاشتراك", nameof(PayDate));
                        OnErrorChanged(nameof(PaymentValue));
                    }
                }
            }
        }
        #endregion
        public SubscriptionCardViewModel? _selectedSubscription;
        public SubscriptionCardViewModel? SelectedSubscription
        {
            get
            {
                return _selectedSubscription;
            }
            set
            {
                _selectedSubscription = value;
                OnPropertyChanged(nameof(SelectedSubscription));
                ClearError(nameof(PaymentValue));
                if (SelectedSubscription != null)
                {

                }

                else
                {
                    AddError("يجب اختيار الاشتراك اولا", nameof(PaymentValue));
                    OnErrorChanged(nameof(PaymentValue));
                }
                ClearError(nameof(PayDate));
                if (SelectedSubscription != null)
                {
                    if (PayDate < Convert.ToDateTime(SelectedSubscription!.RollDate))
                    {
                        AddError("لا يمكن ان يكون تاريخ الدفعة اصغر من تاريخ الاشتراك", nameof(PayDate));
                        OnErrorChanged(nameof(PaymentValue));
                    }
                }
            }
        }
        private void AddSubscriptiont(Subscription subscription)
        {
            SubscriptionCardViewModel itemViewModel =
                new SubscriptionCardViewModel(subscription);
            _subscriptionListViewModel.Add(itemViewModel);
        }

        #region errors
        private void AddError(string? ErrorMsg, string? propertyName)
        {
            if (!PropertyNameToErrorsDictionary.ContainsKey(propertyName!))
            {
                PropertyNameToErrorsDictionary.Add(propertyName!, new List<string>());

            }
            PropertyNameToErrorsDictionary[propertyName!].Add(ErrorMsg!);
            OnErrorChanged(propertyName);
        }

        private void ClearError(string? propertyName)
        {
            PropertyNameToErrorsDictionary.Remove(propertyName!);
            OnErrorChanged(propertyName);
        }

        private void OnErrorChanged(string? PropertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(PropertyName));
            OnPropertyChanged(nameof(CanSubmit));
        }
        public bool CanSubmit => !HasErrors;
        public readonly Dictionary<string, List<string>> PropertyNameToErrorsDictionary;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => PropertyNameToErrorsDictionary.Any();

        public IEnumerable GetErrors(string? propertyName)
        {
            return PropertyNameToErrorsDictionary!.GetValueOrDefault(propertyName, new List<string>());
        }


        #endregion
    }
}

