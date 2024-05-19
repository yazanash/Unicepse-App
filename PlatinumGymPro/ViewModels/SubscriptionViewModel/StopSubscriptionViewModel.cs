using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.SubscriptionViewModel
{
    public class StopSubscriptionViewModel : ErrorNotifyViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayersDataStore _playerDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly PlayerMainPageViewModel _playerMainPageView;
        public SubscriptionCardViewModel? Subscription { get; set; }
        public StopSubscriptionViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playerDataStore, PaymentDataStore paymentDataStore, PlayerMainPageViewModel playerMainPageView)
        {
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _playerDataStore = playerDataStore;
            _paymentDataStore = paymentDataStore;
            _playerMainPageView = playerMainPageView;

            Subscription = new SubscriptionCardViewModel(_subscriptionStore.SelectedSubscription!);

            SubmitCommand = new StopSubscriptionCommand(_subscriptionStore, _playerDataStore, new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageView),this);
        }

        private void CountCoast()
        {
            SubscribeDays = Convert.ToInt32((SubscribeStopDate -Convert.ToDateTime( Subscription!.RollDate)).TotalDays);
            DuesCash = (Subscription.PriceAfterOffer / Subscription.DaysCount) * SubscribeDays;
            ReturnCash = Subscription.PaidValue - DuesCash;
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
                if(SubscribeStopDate < _subscriptionStore.SelectedSubscription!.RollDate)
                {
                    AddError( "لا يمكن ان يكون تاريخ ايقاف الاشتراك اصغر من تاريخ الاشتراك", nameof(SubscribeStopDate));
                    OnErrorChanged(nameof(SubscribeStopDate));
                }
                else
                {
                    CountCoast();
                }
                OnPropertyChanged(nameof(SubscribeStopDate));
            }
        }

        private double _returnCash ;
        public double ReturnCash
        {
            get { return _returnCash; }
            set
            {
                _returnCash = value;
                OnPropertyChanged(nameof(ReturnCash));
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

        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        #endregion
    }
}
