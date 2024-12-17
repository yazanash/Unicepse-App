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
using Unicepse.Stores.RoutineStores;

namespace Unicepse.ViewModels.PlayersAttendenceViewModels
{
    public class LogPlayerAttendenceViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerListItemViewModel> _playerListItemViewModels;
        private readonly PlayersDataStore _playerStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly NavigationStore _navigationStore;

        private readonly HomeViewModel  _homeViewModel;
        private readonly PlayerProfileViewModel _playerProfileViewModel;
        public SearchBoxViewModel SearchBox { get; set; }

        public IEnumerable<PlayerListItemViewModel> PlayersList => _playerListItemViewModels;

        public PlayerListItemViewModel? SelectedPlayer
        {
            get
            {
                return PlayersList
                    .FirstOrDefault(y => y?.Player == _playerStore.SelectedPlayer);
            }
            set
            {
                _playerStore.SelectedPlayer = value?.Player;

            }
        }
        public ICommand LoadPlayersCommand { get; }
        public ICommand CancelCommand { get; }
        public LogPlayerAttendenceViewModel(PlayersDataStore playerStore, PlayersAttendenceStore playersAttendenceStore, NavigationStore navigationStore, HomeViewModel homeViewModel,  PlayerProfileViewModel playerProfileViewModel)
        {
            _playerStore = playerStore;
            LoadPlayersCommand = new LoadPlayersCommand(this, _playerStore);
            _playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();
            _playerStore.Players_loaded += PlayerStore_PlayersLoaded;
            //_subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            _playersAttendenceStore = playersAttendenceStore;
            _navigationStore = navigationStore;
            _homeViewModel = homeViewModel;
            CancelCommand = new NavaigateCommand<HomeViewModel>(new NavigationService<HomeViewModel>(_navigationStore, () => homeViewModel));
            _playerProfileViewModel = playerProfileViewModel;
        }
        private void SearchBox_SearchedText(string? obj)
        {
            _playerListItemViewModels.Clear();
            if (!string.IsNullOrEmpty(obj))
            {
                LoadPlayers(_playerStore.Players, obj);
            }
            else
                LoadPlayers(_playerStore.Players);
        }




        public override void Dispose()
        {
            _playerStore.Players_loaded -= PlayerStore_PlayersLoaded;
            base.Dispose();
        }


        private void PlayerStore_PlayersLoaded()
        {
            _playerListItemViewModels.Clear();

            foreach (Player player in _playerStore.Players)
            {
                AddPlayer(player);
            }
        }
        void LoadPlayers(IEnumerable<Player> players, string query)
        {
            _playerListItemViewModels.Clear();

            foreach (Player player in players.Where(x => x.FullName!.ToLower().Contains(query.ToLower())))
            {
                AddPlayer(player);
            }


        }
        void LoadPlayers(IEnumerable<Player> players)
        {

            _playerListItemViewModels.Clear();

            foreach (Player player in players)
            {
                AddPlayer(player);
            }

        }
        private void AddPlayer(Player player)
        {
            PlayerListItemViewModel itemViewModel =
                new(player, _navigationStore,_playerProfileViewModel , _playerStore, _playersAttendenceStore, _homeViewModel);
            _playerListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _playerListItemViewModels.Count;
        }
        public static LogPlayerAttendenceViewModel LoadViewModel(PlayersDataStore playersStore,
            PlayersAttendenceStore playersAttendenceStore, NavigationStore navigationStore , HomeViewModel homeViewModel,
           
            PlayerProfileViewModel _playerProfileViewModel)
        {
            LogPlayerAttendenceViewModel viewModel = new(playersStore, playersAttendenceStore, navigationStore,homeViewModel, _playerProfileViewModel);

            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }

    }

}
