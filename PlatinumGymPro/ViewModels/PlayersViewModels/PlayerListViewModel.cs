using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.PlayersCommands;
using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels.TrainingViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PlayersViewModels
{
    public class PlayerListViewModel :ViewModelBase
    {
        private readonly ObservableCollection<PlayerListItemViewModel> playerListItemViewModels;
        private NavigationStore _navigatorStore;
        private PlayerStore _playerStore;
        private  TrainerStore _trainerStore;
        private  SportStore _sportStore;
        public IEnumerable<PlayerListItemViewModel> PlayerList => playerListItemViewModels;
        public ICommand AddPlayerCommand { get; }
        public ICommand ActivePlayersCommand { get; }
        public ICommand DeactivePlayersCommand { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public ICommand LoadPlayersCommand { get; }
        public PlayerListViewModel(NavigationStore navigatorStore, PlayerStore playerStore, TrainerStore trainerStore, SportStore sportStore)
        {
            _navigatorStore = navigatorStore;
            _playerStore = playerStore;
            _trainerStore = trainerStore;
            _sportStore = sportStore;
            LoadPlayersCommand = new LoadPlayersCommand(_playerStore, this);
            ActivePlayersCommand = new PlayerByStatusCommand(_playerStore, this, true);
            DeactivePlayersCommand = new PlayerByStatusCommand(_playerStore, this, false);
            AddPlayerCommand = new NavaigateCommand<AddPlayerViewModel>(new NavigationService<AddPlayerViewModel>(_navigatorStore, () => new AddPlayerViewModel(navigatorStore, _playerStore, this)));
            playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();


            _playerStore.PlayersLoaded += _playerStore_PlayersLoaded;
            _playerStore.PlayerAdded += _playerStore_PlayerAdded;
            _playerStore.PlayerUpdated += _playerStore_PlayerUpdated;
            _playerStore.PlayerDeleted += _playerStore_PlayerDeleted;
           
        }
        protected override void Dispose()
        {
            _playerStore.PlayersLoaded -= _playerStore_PlayersLoaded;
            _playerStore.PlayerAdded -= _playerStore_PlayerAdded;
            _playerStore.PlayerUpdated -= _playerStore_PlayerUpdated;
            _playerStore.PlayerDeleted -= _playerStore_PlayerDeleted;
            base.Dispose();
        }
        private void _playerStore_PlayerDeleted(int id)
        {
            PlayerListItemViewModel? itemViewModel = playerListItemViewModels.FirstOrDefault(y => y.Player?.Id == id);

            if (itemViewModel != null)
            {
                playerListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _playerStore_PlayerUpdated(Player player)
        {
            PlayerListItemViewModel? playerViewModel =
                   playerListItemViewModels.FirstOrDefault(y => y.Player.Id == player.Id);

            if (playerViewModel != null)
            {
                playerViewModel.Update(player);
            }
        }

        private void _playerStore_PlayerAdded(Player player)
        {
            AddPlayer(player);
        }

        private void _playerStore_PlayersLoaded()
        {
            playerListItemViewModels.Clear();

            foreach (Player player in _playerStore.Players)
            {
                AddPlayer(player);
            }
        }
        private void AddPlayer (Player player)
        {
            PlayerListItemViewModel itemViewModel =
                new PlayerListItemViewModel(player, _playerStore, _navigatorStore, _trainerStore,_sportStore,this);
            playerListItemViewModels.Add(itemViewModel);
        }
        public static PlayerListViewModel LoadViewModel(PlayerStore playerStore, NavigationStore navigatorStore, TrainerStore trainerStore, SportStore sportStore)
        {
            PlayerListViewModel viewModel = new PlayerListViewModel(navigatorStore, playerStore, trainerStore, sportStore);

            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }
    }
}
