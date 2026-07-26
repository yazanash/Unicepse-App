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
                if (!_stopSubscription.SelectedSubscription!.IsStopped)
                {
                    _stopSubscription.SelectedSubscription!.PriceAfterOffer -= _stopSubscription.ReturnCash;
                    
                    int days = Convert.ToInt32((_stopSubscription.SubscribeStopDate - _stopSubscription.SelectedSubscription!.RollDate).TotalDays);
                    double dayPrice = _stopSubscription.SelectedSubscription!.PriceAfterOffer / _stopSubscription.SelectedSubscription!.DaysCount;
                    _stopSubscription.SelectedSubscription!.PriceAfterOffer = dayPrice * days;
              
                    await _subscriptionDataStore.Stop(_stopSubscription.SelectedSubscription!, _stopSubscription.SubscribeStopDate);
                    Subscription? subscription = _subscriptionDataStore.Subscriptions.OrderByDescending(x => x.EndDate).FirstOrDefault(x => x.Id != _stopSubscription.SelectedSubscription.Id);
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
