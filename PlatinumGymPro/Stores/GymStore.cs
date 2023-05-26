using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class GymStore
    {
        private readonly List<Player> _players;
        private readonly Lazy<Task> _initializLazy;
        public event Action<Player> PlayerMade;
        public IEnumerable<Player> Players => _players;

        private readonly Gym _gym;

        public GymStore(Gym gym)
        {
            _players = new List<Player>();
            _gym = gym;
            _initializLazy = new Lazy<Task>(Initialize);
        }
        public async Task Load()
        {
            await _initializLazy.Value;

        }
        public async Task MakePlayer(Player player)
        {
            await _gym.AddPlayer(player);
            _players.Add(player);
            OnPlayerMade(player);
        }

        private void OnPlayerMade(Player player)
        {
            PlayerMade?.Invoke(player);
        }

        private async Task Initialize()
        {
            IEnumerable<Player> players = await _gym.GetPlayers();
            _players.Clear();
            _players.AddRange(players);
        }
    }
}
