using Unicepse.Core.Models.Player;
using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.navigation.Stores;
using Unicepse.utlis.common;
using Unicepse.ViewModels.SubscriptionViewModel;
using Unicepse.Core.Models.Subscription;
using Unicepse.Commands.SubscriptionCommand;
using Unicepse.navigation;

namespace Unicepse.ViewModels.PlayersAttendenceViewModels
{
    public class LogPlayerAttendenceViewModel : ListingViewModelBase

    {
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        private readonly PlayersDataStore _playerStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly NavigationStore _navigationStore;
        private readonly HomeViewModel  _homeViewModel;
        public SearchBoxViewModel SearchBox { get; set; }

        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => _subscriptionListItemViewModels;

        public SubscriptionListItemViewModel? SelectedSubscription
        {
            get
            {
                return SubscriptionList
                    .FirstOrDefault(y => y?.Subscription == _subscriptionDataStore.SelectedSubscription);
            }
            set
            {
                _subscriptionDataStore.SelectedSubscription = value?.Subscription;

            }
        }
        public ICommand LoadPlayersCommand { get; }
        public ICommand CancelCommand { get; }
        public LogPlayerAttendenceViewModel(PlayersDataStore playerStore, PlayersAttendenceStore playersAttendenceStore, NavigationStore navigationStore, HomeViewModel homeViewModel, SubscriptionDataStore subscriptionDataStore)
        {
            _playerStore = playerStore;
            _subscriptionDataStore = subscriptionDataStore;
            LoadPlayersCommand = new LoadActiveSubscriptionCommand( _subscriptionDataStore, this);
            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            //_playerStore.Players_loaded += PlayerStore_PlayersLoaded;
            _subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            _playersAttendenceStore = playersAttendenceStore;
            _navigationStore = navigationStore;
            _homeViewModel = homeViewModel;
            CancelCommand = new NavaigateCommand<HomeViewModel>(new NavigationService<HomeViewModel>(_navigationStore, () => homeViewModel));
        }

        private void _subscriptionDataStore_Loaded()
        {
            _subscriptionListItemViewModels.Clear();

            foreach (Subscription  subscription in _subscriptionDataStore.Subscriptions)
            {
                AddSubscription(subscription);
            }
        }

        private void SearchBox_SearchedText(string? obj)
        {
            _subscriptionListItemViewModels.Clear();
            if (!string.IsNullOrEmpty(obj))
            {
                LoadSubscription(_subscriptionDataStore.Subscriptions, obj);
            }
            else
                LoadSubscription(_subscriptionDataStore.Subscriptions);
        }




        public override void Dispose()
        {
            _playerStore.Players_loaded -= PlayerStore_PlayersLoaded;
            base.Dispose();
        }


        private void PlayerStore_PlayersLoaded()
        {
            //playerListItemViewModels.Clear();

            //foreach (Player player in _playerStore.Players)
            //{
            //    AddPlayer(player);
            //}
        }
        void LoadSubscription(IEnumerable<Subscription> subscriptions, string query)
        {
            _subscriptionListItemViewModels.Clear();

            foreach (Subscription subscription in subscriptions.Where(x => x.Player!.FullName!.ToLower().Contains(query.ToLower())))
            {
                AddSubscription(subscription);
            }


        }
        void LoadSubscription(IEnumerable<Subscription> subscriptions)
        {

            _subscriptionListItemViewModels.Clear();

            foreach (Subscription subscription in subscriptions)
            {
                AddSubscription(subscription);
            }

        }
        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel itemViewModel =
                new(subscription,_subscriptionDataStore,_playersAttendenceStore,_navigationStore,_homeViewModel);
            _subscriptionListItemViewModels.Add(itemViewModel);
        }
        public static LogPlayerAttendenceViewModel LoadViewModel(PlayersDataStore playersStore, PlayersAttendenceStore playersAttendenceStore, NavigationStore navigationStore , HomeViewModel homeViewModel,SubscriptionDataStore subscriptionDataStore)
        {
            LogPlayerAttendenceViewModel viewModel = new(playersStore, playersAttendenceStore, navigationStore,homeViewModel, subscriptionDataStore);

            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }

    }

}
