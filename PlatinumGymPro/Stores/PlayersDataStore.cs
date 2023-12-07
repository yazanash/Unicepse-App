using PlatinumGym.Core.Models.Player;
using PlatinumGym.Entityframework.Services.PlayerQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public enum Filter
    {
        ByStatus,
        ByGender,
        ByDebt,
        BySubscribeEnd
    }
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


        public async Task GetPlayers()
        {
            await _initializeLazy.Value;
        }

        public async Task GetPlayers(Filter filter)
        {
            IEnumerable<Player>? players;
            switch (filter)
            {
                case Filter.BySubscribeEnd:
                    players = await _playerDataService.GetBySubscribeEnd();
                    RearrengeList(players);
                    break;
                case Filter.ByDebt:
                    players = await _playerDataService.GetByDebt();
                    RearrengeList(players);
                    break;
            }
           
        }
        public void RearrengeList(IEnumerable<Player> players )
        {
            _players.Clear();
            _players.AddRange(players!);
            players_loaded?.Invoke();
        }
        public async Task GetPlayers(Filter filter , bool statusOrGender)
        {
            IEnumerable<Player> players;
            switch (filter)
            {
                case Filter.ByStatus:
                    players = await _playerDataService.GetByStatus(statusOrGender);
                    RearrengeList(players);
                    break;
                case Filter.ByGender:
                    players = await _playerDataService.GetByGender(statusOrGender);
                    RearrengeList(players);
                    break;
                default:
                    players = await _playerDataService.GetByStatus(statusOrGender);
                    RearrengeList(players);
                    break;
            }
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
