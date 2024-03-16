using PlatinumGym.Core.Models.Player;
using PlatinumGym.Entityframework.Services.PlayerQueries;
using PlatinumGymPro.Enums;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
 
    public class PlayersDataStore
    {
        private readonly PlayerDataService _playerDataService;
        private readonly List<Player> _players;
        private readonly Lazy<Task> _initializeLazy;

        public IEnumerable<Player> Players => _players;
        public event Action<Player>? Player_created;
        public event Action? Players_loaded;
        public event Action<Player>? Player_update;
        public event Action<int>? Player_deleted;


        public event Action<Filter?>? FilterChanged;
        public event Action<PlayerListItemViewModel?>? PlayerChanged;
        public PlayersDataStore(PlayerDataService playerDataService)
        {
            _playerDataService = playerDataService;
            _players = new List<Player>();
            _initializeLazy = new Lazy<Task>(Initialize);
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

        public void RearrengeList(IEnumerable<Player> players )
        {
            _players.Clear();
            _players.AddRange(players!);
            Players_loaded?.Invoke();
        }
        public async Task AddPlayer(Player player)
        {
            await _playerDataService.Create(player);
            _players.Add(player);
            Player_created?.Invoke(player);
        }
        public async Task UpdatePlayer(Player player)
        {
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
    }
}
