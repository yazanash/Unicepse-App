using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using PlatinumGymPro.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.SubscriptionCommand
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
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            await _subscriptionDataStore.Stop(_subscriptionDataStore.SelectedSubscription!, _stopSubscription.SubscribeStopDate);
            MessageBox.Show(_subscriptionDataStore.SelectedSubscription!.Sport!.Name + " Stopped successfully");

            _navigationService.ReNavigate();
        }
    }
}
