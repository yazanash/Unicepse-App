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
    public class MoveToNewTrainerCommand : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly NavigationService<PlayerMainPageViewModel> _navigationService;
        private readonly MoveToNewTrainerViewModel _moveToNewTrainerViewModel;

        public MoveToNewTrainerCommand(SubscriptionDataStore subscriptionDataStore, NavigationService<PlayerMainPageViewModel> navigationService, MoveToNewTrainerViewModel moveToNewTrainerViewModel)
        {
            _subscriptionDataStore = subscriptionDataStore;
            _navigationService = navigationService;
            _moveToNewTrainerViewModel = moveToNewTrainerViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            await _subscriptionDataStore.MoveToNewTrainer(_subscriptionDataStore.SelectedSubscription!,_subscriptionDataStore.SelectedTrainer!, _moveToNewTrainerViewModel.MoveDate);
            MessageBox.Show(_subscriptionDataStore.SelectedSubscription!.Sport!.Name + " moved successfully");

            _navigationService.ReNavigate();
        }
    }
}
