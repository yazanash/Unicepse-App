using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Models;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Services;

namespace Unicepse.API.Services
{
    public class PlayerApiDataService : IApiDataService<Player>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public PlayerApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<bool> Create(Player entity)
        {
            PlayerDto playerDto = new PlayerDto();
            playerDto.gym_id = 18;
            playerDto.FromPlayer(entity);
            return await _client.PostAsync("player",playerDto);
             
        }
        public async Task<Player> Get(Player entity)
        {
            PlayerDto player = await _client.GetAsync<PlayerDto>($"player/18/{entity.Id}");
            return player.ToPlayer();
        }
        public async Task<bool> Update(Player entity)
        {
            PlayerDto playerDto = new PlayerDto();
            playerDto.gym_id = 18;
            playerDto.FromPlayer(entity);
            return await _client.PutAsync("player", playerDto);
        }
    }
}
