using PlatinumGym.Core.Models.Player;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PlayersAttendenceViewModels
{
    public class LogPlayerAttendenceViewModel : ListingViewModelBase

    {
        private readonly ObservableCollection<PlayerListItemViewModel> playerListItemViewModels;
        private readonly PlayersDataStore _playerStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;

        public SearchBoxViewModel SearchBox { get; set; }

        public IEnumerable<PlayerListItemViewModel> PlayerList => playerListItemViewModels;

        public PlayerListItemViewModel? SelectedPlayer
        {
            get
            {
                return _playerStore.SelectedPlayer;
            }
            set
            {
                _playerStore.SelectedPlayer = value;

            }
        }
        public ICommand LoadPlayersCommand { get; }
        public LogPlayerAttendenceViewModel(PlayersDataStore playerStore, PlayersAttendenceStore playersAttendenceStore)
        {
            _playerStore = playerStore;
            LoadPlayersCommand = new LoadPlayersCommand(this, _playerStore);
            playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();
            _playerStore.Players_loaded += PlayerStore_PlayersLoaded;

            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            _playersAttendenceStore = playersAttendenceStore;
        }

        private void SearchBox_SearchedText(string? obj)
        {
            playerListItemViewModels.Clear();
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
            LoadPlayers(_playerStore.Players);
        }
        void LoadPlayers(IEnumerable<Player> players, string query)
        {
            playerListItemViewModels.Clear();

            foreach (Player player in players.Where(x => x.FullName!.ToLower().Contains(query.ToLower())))
            {
                AddPlayer(player);
            }
        

        }
        void LoadPlayers(IEnumerable<Player> players)
        {
            playerListItemViewModels.Clear();

            foreach (Player player in players)
            {
                AddPlayer(player);
            }


        }
        private void AddPlayer(Player player)
        {
            PlayerListItemViewModel itemViewModel =
                new(player, _playerStore, _playersAttendenceStore);
            playerListItemViewModels.Add(itemViewModel);
        }
        public static LogPlayerAttendenceViewModel LoadViewModel(PlayersDataStore playersStore , PlayersAttendenceStore playersAttendenceStore)
        {
            LogPlayerAttendenceViewModel viewModel = new(playersStore, playersAttendenceStore);

            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }

    }

}
