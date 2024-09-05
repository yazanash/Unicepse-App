using Unicepse.Core.Models.Player;
using Unicepse.Entityframework.Services.PlayerQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.API.Services;
using Unicepse.Core.Common;
using Unicepse.BackgroundServices;

namespace Unicepse.Stores
{

    public class PlayersDataStore
    {
        private readonly PlayerDataService _playerDataService;
        private readonly PlayerApiDataService _playerApiDataService;
        private readonly List<Player> _players;
        private readonly Lazy<Task> _initializeLazy;

        public IEnumerable<Player> Players => _players;
        public event Action<Player>? Player_created;
        public event Action? Players_loaded;
        public event Action<Player>? Player_update;
        public event Action<int>? Player_deleted;

        public event Action<Profile>? profile_loaded;


        public event Action<Filter?>? FilterChanged;
        public event Action<PlayerListItemViewModel?>? PlayerChanged;
        public PlayersDataStore(PlayerDataService playerDataService, PlayerApiDataService playerApiDataService)
        {
            _playerDataService = playerDataService;
            _players = new List<Player>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _playerApiDataService = playerApiDataService;
        }

        private PlayerListItemViewModel? _selectedPlayer;
        public PlayerListItemViewModel? SelectedPlayer
        {
            get
            {
                return _selectedPlayer;
            }
            set
            {
                _selectedPlayer = value;
                PlayerChanged?.Invoke(_selectedPlayer);
            }
        }

        private Filter? _selectedFilter;
        public Filter? SelectedFilter
        {
            get
            {
                return _selectedFilter;
            }
            set
            {
                _selectedFilter = value;
                FilterChanged?.Invoke(_selectedFilter);
            }
        }


        private Order? _selectedOrder;
        public Order? SelectedOrder
        {
            get
            {
                return _selectedOrder;
            }
            set
            {
                _selectedOrder = value;
                OrderChanged?.Invoke(_selectedOrder);
            }
        }

        public event Action<Order?>? OrderChanged;
        public async Task GetPlayers()
        {
            await _initializeLazy.Value;
            Players_loaded?.Invoke();
        }

        public void RearrengeList(IEnumerable<Player> players)
        {
            _players.Clear();
            _players.AddRange(players!);
            Players_loaded?.Invoke();
        }
        public async Task HandShakePlayer(Player player,string uid)
        {
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                bool status = await _playerApiDataService.CreateHandShake(player,uid);
                if (status)
                {
                    player.UID = uid;
                    await _playerDataService.Update(player);
                    int currentIndex = _players.FindIndex(y => y.Id == player.Id);

                    if (currentIndex != -1)
                    {
                        _players[currentIndex] = player;
                    }
                    else
                    {
                        _players.Add(player);
                    }
                    Player_update?.Invoke(player);
                }

            }
            _players.Add(player);
            Player_created?.Invoke(player);
        }
        public async Task GetPlayerProfile(string uid)
        {
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                Profile profile = await _playerApiDataService.GetProfile(uid);
                if (profile!=null)
                {
                    profile_loaded?.Invoke(profile);
                }
            }
        }
        public async Task AddPlayer(Player player)
        {
            player.DataStatus = DataStatus.ToCreate;
            await _playerDataService.Create(player);
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                bool status = await _playerApiDataService.Create(player);
                if (status)
                {
                    player.DataStatus = DataStatus.Synced;
                    await _playerDataService.Update(player);
                }

            }
            _players.Add(player);
            Player_created?.Invoke(player);
        }
        public async Task UpdatePlayer(Player player)
        {
            if (player.DataStatus != DataStatus.ToCreate)
                player.DataStatus = DataStatus.ToUpdate;
            await _playerDataService.Update(player);

            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                bool status = await _playerApiDataService.Update(player);
                if (status)
                {
                    player.DataStatus = DataStatus.Synced;
                    await _playerDataService.Update(player);
                }

            }


            int currentIndex = _players.FindIndex(y => y.Id == player.Id);

            if (currentIndex != -1)
            {
                _players[currentIndex] = player;
            }
            else
            {
                _players.Add(player);
            }
            Player_update?.Invoke(player);
        }
        public async Task DeletePlayer(Player player)
        {
            await _playerDataService.Update(player);
            int currentIndex = _players.FindIndex(y => y.Id == player.Id);
            _players.RemoveAt(currentIndex);
            Player_deleted?.Invoke(player.Id);
        }

        public async Task ForceDeletePlayer(int player_id)
        {
            bool deleted = await _playerDataService.Delete(player_id);
            int currentIndex = _players.FindIndex(y => y.Id == player_id);
            _players.RemoveAt(currentIndex);
            Player_deleted?.Invoke(player_id);
        }


        private async Task Initialize()
        {
            IEnumerable<Player> players = await _playerDataService.GetByStatus(true);
            RearrengeList(players);
        }

        public async Task SyncPlayersToCreate()
        {
            IEnumerable<Player> players = await _playerDataService.GetByDataStatus(DataStatus.ToCreate);
            foreach (Player player in players)
            {
                bool status = await _playerApiDataService.Create(player);
                if (status)
                {
                    player.DataStatus = DataStatus.Synced;
                    await _playerDataService.Update(player);
                }


            }
        }

        public async Task SyncPlayersToUpdate()
        {
            IEnumerable<Player> players = await _playerDataService.GetByDataStatus(DataStatus.ToUpdate);
            foreach (Player player in players)
            {
                bool status = await _playerApiDataService.Update(player);
                if (status)
                {
                    player.DataStatus = DataStatus.Synced;
                    await _playerDataService.Update(player);
                }


            }
        }
    }
}
