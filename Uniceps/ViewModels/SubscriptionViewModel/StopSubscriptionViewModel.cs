using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.ViewModels;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
    public class StopSubscriptionViewModel : ErrorNotifyViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayersDataStore _playerDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly PlayerMainPageViewModel _playerMainPageView;
        public Subscription SelectedSubscription;
        public SubscriptionCardViewModel? Subscription { get; set; }
        public StopSubscriptionViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playerDataStore, PaymentDataStore paymentDataStore, PlayerMainPageViewModel playerMainPageView, Subscription selectedSubscription)
        {
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _playerDataStore = playerDataStore;
            _paymentDataStore = paymentDataStore;
            _playerMainPageView = playerMainPageView;
            SelectedSubscription = selectedSubscription;

            FromRef = true;
            Subscription = new SubscriptionCardViewModel(SelectedSubscription);
            CancelCommand = new NavaigateCommand<PlayerMainPageViewModel>(new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageView));

            SubmitCommand = new StopSubscriptionCommand(_subscriptionStore, _playerDataStore, new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageView), this);
        }

        private void CountCoast()
        {
            SubscribeDays = Convert.ToInt32((SubscribeStopDate - Convert.ToDateTime(SelectedSubscription.RollDate)).TotalDays);
            DuesCash = SelectedSubscription.PriceAfterOffer / SelectedSubscription.DaysCount * SubscribeDays;
            //ReturnCash = _subscriptionStore.SelectedSubscription!.PaidValue - DuesCash;
        }
        private void CountCoastFromDays()
        {
            SubscribeStopDate = SelectedSubscription.RollDate.AddDays(SubscribeDays);
            DuesCash = SelectedSubscription.PriceAfterOffer / SelectedSubscription.DaysCount * SubscribeDays;
            //ReturnCash = _subscriptionStore.SelectedSubscription!.PaidValue - DuesCash;
        }
        private void CountCoastFromRef()
        {
            //SubscribeStopDate = _subscriptionStore.SelectedSubscription!.RollDate.AddDays(SubscribeDays);
            int Price = Convert.ToInt32(SelectedSubscription.PriceAfterOffer / SelectedSubscription.DaysCount);
            //DuesCash = _subscriptionStore.SelectedSubscription!.PaidValue - ReturnCash;
            SubscribeDays = Convert.ToInt32(DuesCash / Price);
            SubscribeStopDate = SelectedSubscription.RollDate.AddDays(SubscribeDays);
        }
        #region Properties 
        private int _subscribeDays;
        public int SubscribeDays
        {
            get { return _subscribeDays; }
            set
            {
                _subscribeDays = value;
                OnPropertyChanged(nameof(SubscribeDays));
                if (FromDays)
                    CountCoastFromDays();
            }
        }
        private DateTime _subscribeStopDate = DateTime.Now;
        public DateTime SubscribeStopDate
        {
            get { return _subscribeStopDate; }
            set
            {
                _subscribeStopDate = value;
                OnPropertyChanged(nameof(SubscribeStopDate));
                ClearError(nameof(SubscribeStopDate));
                if (SubscribeStopDate < SelectedSubscription.RollDate)
                {
                    AddError("لا يمكن ان يكون تاريخ ايقاف الاشتراك اصغر من تاريخ الاشتراك", nameof(SubscribeStopDate));
                    OnErrorChanged(nameof(SubscribeStopDate));
                }
                else if (SubscribeStopDate >= SelectedSubscription.EndDate)
                {
                    AddError("لا يمكن ان يكون تاريخ ايقاف الاشتراك اكبر من تاريخ نهاية الاشتراك", nameof(SubscribeStopDate));
                    OnErrorChanged(nameof(SubscribeStopDate));
                }
                else
                {
                    if (FromDate)
                        CountCoast();
                }
                OnPropertyChanged(nameof(SubscribeStopDate));
            }
        }

        private double _returnCash;
        public double ReturnCash
        {
            get { return _returnCash; }
            set
            {
                _returnCash = value;
                OnPropertyChanged(nameof(ReturnCash));
                if (FromRef)
                    CountCoastFromRef();
            }
        }
        private double _duesCash;
        public double DuesCash
        {
            get { return _duesCash; }
            set
            {
                _duesCash = value;
                OnPropertyChanged(nameof(DuesCash));
            }
        }
        private bool _fromDays;
        public bool FromDays
        {
            get { return _fromDays; }
            set
            {
                _fromDays = value;
                OnPropertyChanged(nameof(FromDays));
            }
        }
        private bool _fromRef;
        public bool FromRef
        {
            get { return _fromRef; }
            set
            {
                _fromRef = value;
                OnPropertyChanged(nameof(FromRef));
            }
        }
        private bool _fromDate;
        public bool FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
            }
        }
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        #endregion
    }
}
