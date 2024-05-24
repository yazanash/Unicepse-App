using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.Payments;
using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SubscriptionViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PaymentsViewModels
{
    public class EditPaymentViewModel : ListingViewModelBase, INotifyDataErrorInfo
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
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();

            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionDataStore, _playersDataStore.SelectedPlayer!);
            _subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            SubmitCommand = new EditPaymentsCommand(new NavigationService<PaymentListViewModel>(_navigatorStore, () => _paymentListViewModel), _paymentDataStore, this, _playersDataStore, _subscriptionDataStore);
            CancelCommand = new NavaigateCommand<PaymentListViewModel>(new NavigationService<PaymentListViewModel>(_navigatorStore, () => _paymentListViewModel));

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
        public ICommand CancelCommand { get; }
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
                    if (PaymentValue > SelectedSubscription!.PriceAfterOffer - SelectedSubscription!.PaidValue +_paymentDataStore.SelectedPayment!.PaymentValue)
                    {
                        AddError("لا يمكن ان يكون المبلغ المدفوع اكبر من المستحق", nameof(PaymentValue));
                        OnErrorChanged(nameof(PaymentValue));
                    }
                    else if (PaymentValue < 0)
                    {
                        AddError("لايمكن الدفع بقيمة اقل من 0", nameof(PaymentValue));
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
                ClearError(nameof(PaymentValue));
                if (SelectedSubscription != null)
                {
                    if (PaymentValue > SelectedSubscription!.PriceAfterOffer - SelectedSubscription!.PaidValue)
                    {
                        AddError("لا يمكن ان يكون المبلغ المدفوع اكبر من المستحق", nameof(PaymentValue));
                        OnErrorChanged(nameof(PaymentValue));
                    }
                }

                else
                {
                    AddError("يجب اختيار الاشتراك اولا", nameof(PaymentValue));
                    OnErrorChanged(nameof(PaymentValue));
                }
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
