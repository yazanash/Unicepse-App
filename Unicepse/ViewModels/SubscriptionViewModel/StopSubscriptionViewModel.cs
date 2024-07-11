﻿using Unicepse.Commands.SubscriptionCommand;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.SubscriptionViewModel
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
            CancelCommand = new NavaigateCommand<PlayerMainPageViewModel>(new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageView));

            SubmitCommand = new StopSubscriptionCommand(_subscriptionStore, _playerDataStore, new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageView), this);
        }

        private void CountCoast()
        {
            //SubscribeDays = Convert.ToInt32((SubscribeStopDate - Convert.ToDateTime(_subscriptionStore.SelectedSubscription!.RollDate)).TotalDays);
            DuesCash = _subscriptionStore.SelectedSubscription!.PriceAfterOffer / _subscriptionStore.SelectedSubscription!.DaysCount * SubscribeDays;
            ReturnCash = _subscriptionStore.SelectedSubscription!.PaidValue - DuesCash;
        }
        private void CountCoastFromDays()
        {
           SubscribeStopDate=_subscriptionStore.SelectedSubscription!.RollDate.AddDays(SubscribeDays);
            DuesCash = _subscriptionStore.SelectedSubscription!.PriceAfterOffer / _subscriptionStore.SelectedSubscription!.DaysCount * SubscribeDays;
            ReturnCash = _subscriptionStore.SelectedSubscription!.PaidValue - DuesCash;
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
                if (SubscribeStopDate < _subscriptionStore.SelectedSubscription!.RollDate)
                {
                    AddError("لا يمكن ان يكون تاريخ ايقاف الاشتراك اصغر من تاريخ الاشتراك", nameof(SubscribeStopDate));
                    OnErrorChanged(nameof(SubscribeStopDate));
                }
                else if (SubscribeStopDate >= _subscriptionStore.SelectedSubscription!.EndDate)
                {
                    AddError("لا يمكن ان يكون تاريخ ايقاف الاشتراك اكبر من تاريخ نهاية الاشتراك", nameof(SubscribeStopDate));
                    OnErrorChanged(nameof(SubscribeStopDate));
                }
                else
                {
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
