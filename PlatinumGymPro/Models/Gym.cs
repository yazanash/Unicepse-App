using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class Gym 
    {
        private readonly PlayersBook _PlayersBook;
        public Gym(PlayersBook playersBook)
        {
            _PlayersBook = playersBook;
        }
        public async Task< IEnumerable<Player>> GetPlayers()
        {
            return await _PlayersBook.GetPlayers();
        }
        public async Task AddPlayer(Player player)
        {
           await  _PlayersBook.AddPlayer(player);
        }
    }
}
