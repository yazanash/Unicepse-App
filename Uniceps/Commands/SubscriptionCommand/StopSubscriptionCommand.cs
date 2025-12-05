using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Commands.SubscriptionCommand
{
    public class StopSubscriptionCommand : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PlayersDataStore _playerDataStore;
        private readonly NavigationService<PlayerMainPageViewModel> _navigationService;
        private readonly StopSubscriptionViewModel _stopSubscription;

        public StopSubscriptionCommand(SubscriptionDataStore subscriptionDataStore, PlayersDataStore playerDataStore, NavigationService<PlayerMainPageViewModel> navigationService, StopSubscriptionViewModel stopSubscription)
        {
            _subscriptionDataStore = subscriptionDataStore;
            _playerDataStore = playerDataStore;
            _navigationService = navigationService;
            _stopSubscription = stopSubscription;
            _stopSubscription.PropertyChanged += _stopSubscription_PropertyChanged;
        }

        private void _stopSubscription_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_stopSubscription.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }
        public override bool CanExecute(object? parameter)
        {

            return _stopSubscription.CanSubmit && base.CanExecute(null);
        }
        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                if (!_subscriptionDataStore.SelectedSubscription!.IsStopped)
                {
                    _playerDataStore.SelectedPlayer!.Balance += _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer;
                    _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer -= _stopSubscription.ReturnCash;
                    
                    int days = Convert.ToInt32((_stopSubscription.SubscribeStopDate - _subscriptionDataStore.SelectedSubscription!.RollDate).TotalDays);
                    double dayPrice = _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer / _subscriptionDataStore.SelectedSubscription!.DaysCount;
                    _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer = dayPrice * days;
                    _playerDataStore.SelectedPlayer!.Balance -= _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer;
                    await _subscriptionDataStore.Stop(_subscriptionDataStore.SelectedSubscription!, _stopSubscription.SubscribeStopDate);
                    Subscription? subscription = _subscriptionDataStore.Subscriptions.OrderByDescending(x => x.EndDate).FirstOrDefault(x => x.Id != _subscriptionDataStore.SelectedSubscription.Id);
                    if (subscription != null && subscription.EndDate >= _subscriptionDataStore.SelectedSubscription.EndDate)
                    {
                        _playerDataStore.SelectedPlayer!.SubscribeEndDate = subscription.EndDate;
                    }
                    else
                        _playerDataStore.SelectedPlayer!.SubscribeEndDate = _subscriptionDataStore.SelectedSubscription.EndDate;
                    await _playerDataStore.UpdatePlayer(_playerDataStore.SelectedPlayer);
                    _navigationService.ReNavigate();
                }
                else
                {
                    MessageBox.Show("هذا الاشتراك تم ايقافه سابقا");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
