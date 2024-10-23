using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels.SubscriptionViewModel;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.navigation;

namespace Unicepse.Commands.SubscriptionCommand
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
                    _playerDataStore.SelectedPlayer!.Player!.Balance += _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer;
                    _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer -= _stopSubscription.ReturnCash;
                    //entity.PriceAfterOffer = dayPrice * days;
                    //int days = Convert.ToInt32((_stopSubscription.SubscribeStopDate - _subscriptionDataStore.SelectedSubscription!.RollDate).TotalDays);
                    //double dayPrice = _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer / _subscriptionDataStore.SelectedSubscription!.Sport!.DaysCount;
                    _playerDataStore.SelectedPlayer!.Player.Balance -= _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer;
                    await _subscriptionDataStore.Stop(_subscriptionDataStore.SelectedSubscription!, _stopSubscription.SubscribeStopDate);
                    await _playerDataStore.UpdatePlayer(_playerDataStore.SelectedPlayer.Player);
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
