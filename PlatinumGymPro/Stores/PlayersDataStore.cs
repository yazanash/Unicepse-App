using PlatinumGym.Core.Models.Player;
using PlatinumGym.Entityframework.Services.PlayerQueries;
using PlatinumGymPro.Enums;
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
        public event Action<Player>? player_created;
        public event Action? players_loaded;
        public event Action<Player>? player_update;
        public event Action<int>? player_deleted;
        public PlayersDataStore(PlayerDataService playerDataService)
        {
            _playerDataService = playerDataService;
            _players = new List<Player>();
            _initializeLazy = new Lazy<Task>(Initialize);
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

        public event Action<Filter?>? FilterChanged;
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
            players_loaded?.Invoke();
        }

        public void RearrengeList(IEnumerable<Player> players )
        {
            _players.Clear();
            _players.AddRange(players!);
            players_loaded?.Invoke();
        }
     

        public async Task AddPlayer(Player player)
        {
            await _playerDataService.Create(player);
            _players.Add(player);
            player_created?.Invoke(player);
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
            player_update?.Invoke(player);
        }

        public async Task DeletePlayer(int player_id)
        {
            bool deleted = await _playerDataService.Delete(player_id);
            int currentIndex = _players.FindIndex(y => y.Id == player_id);
            _players.RemoveAt(currentIndex);
            player_deleted?.Invoke(player_id);
        }


        private async Task Initialize()
        {
            IEnumerable<Player> players = await _playerDataService.GetByStatus(true);
            RearrengeList(players);
        }
    }
}
