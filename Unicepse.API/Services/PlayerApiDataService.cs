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
        public async Task<bool> CreateHandShake(Player entity,string uid)
        {
            HandShakeDto handShakeDto = new HandShakeDto();
            handShakeDto.gym_id = "18";
            handShakeDto.pid = entity.Id.ToString();
            handShakeDto.uid = uid;
            return await _client.PostAsync("handshake", handShakeDto);

        }
        public async Task<Player> Get(Player entity)
        {
            PlayerDto player = await _client.GetAsync<PlayerDto>($"player/18/{entity.Id}");
            return player.ToPlayer();
        }

        public async Task<Profile> GetProfile(string? uid)
        {
            ProfileDto profile = await _client.GetAsync<ProfileDto>($"profile/{uid}");
            return profile.ToProfile();
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
