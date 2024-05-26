using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.PlayersViewModels;
using Unicepse.WPF.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.WPF.navigation;

namespace Unicepse.WPF.Commands.SubscriptionCommand
{
    public class StopSubscriptionCommand :AsyncCommandBase
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
                await _subscriptionDataStore.Stop(_subscriptionDataStore.SelectedSubscription!, _stopSubscription.SubscribeStopDate);
                _navigationService.ReNavigate();
            }
           catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
