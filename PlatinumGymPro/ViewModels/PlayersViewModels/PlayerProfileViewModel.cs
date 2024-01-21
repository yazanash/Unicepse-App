using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PlayersViewModels
{
    public class PlayerProfileViewModel : ViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SportDataStore _sportDataStore;
        public PlayerListItemViewModel? Player => _playersDataStore.SelectedPlayer;
        public ViewModelBase? CurrentPlayerViewModel => _navigatorStore.CurrentViewModel;

        public PlayerProfileViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore,
            PlayersDataStore playersDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore)
        {
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _playersDataStore = playersDataStore;
            _sportDataStore = sportDataStore;
            _paymentDataStore = paymentDataStore;

            navigatorStore.CurrentViewModel = LoadPlayerMainPageViewModel(_navigatorStore, _playersDataStore, _subscriptionStore, _paymentDataStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            _playersDataStore.PlayerChanged += _playersDataStore_PlayerChanged;
            //PlayerHomeCommand = new NavaigateCommand<SubscriptionDetailsViewModel>(new NavigationService<SubscriptionDetailsViewModel>(_navigatorStore, () => new SubscriptionDetailsViewModel()));
            SubscriptionCommand = new NavaigateCommand<SubscriptionDetailsViewModel>(new NavigationService<SubscriptionDetailsViewModel>(_navigatorStore, () => LoadSubscriptionViewModel(_navigatorStore, _sportDataStore,_subscriptionStore,_playersDataStore,_paymentDataStore)));
            //PaymentCommand = new NavaigateCommand<SubscriptionDetailsViewModel>(new NavigationService<SubscriptionDetailsViewModel>(_navigatorStore, () => new SubscriptionDetailsViewModel()));
            //MetricsCommand = new NavaigateCommand<SubscriptionDetailsViewModel>(new NavigationService<SubscriptionDetailsViewModel>(_navigatorStore, () => new SubscriptionDetailsViewModel()));
            //TrainingProgramCommand = new NavaigateCommand<SubscriptionDetailsViewModel>(new NavigationService<SubscriptionDetailsViewModel>(_navigatorStore, () => new SubscriptionDetailsViewModel()));
        }

        private void _playersDataStore_PlayerChanged(PlayerListItemViewModel? obj)
        {
            OnPropertyChanged(nameof(Player));
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentPlayerViewModel));
        }
        private PlayerMainPageViewModel LoadPlayerMainPageViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionDataStore,PaymentDataStore paymentDataStore)
        {
            return PlayerMainPageViewModel.LoadViewModel(navigatorStore, subscriptionDataStore, playerStore, paymentDataStore);
        }

        private SubscriptionDetailsViewModel LoadSubscriptionViewModel(NavigationStore navigatorStore, SportDataStore sportDataStore,SubscriptionDataStore subscriptionDataStore,PlayersDataStore playersDataStore,PaymentDataStore paymentDataStore)
        {
            return SubscriptionDetailsViewModel.LoadViewModel(sportDataStore,navigatorStore, subscriptionDataStore, playersDataStore, paymentDataStore);
        }
        public ICommand? PlayerHomeCommand { get; }
        public ICommand? SubscriptionCommand { get; }
        public ICommand? PaymentCommand { get; }
        public ICommand? MetricsCommand { get; }
        public ICommand? TrainingProgramCommand { get; }
    }
}
