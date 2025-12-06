using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API;
using Uniceps.API.Models;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Services;

namespace Uniceps.API.Services
{
    public class PlayerApiDataService : IApiDataService<PlayerDto>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public PlayerApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<int> Create(PlayerDto entity)
        {
            //PlayerDto playerDto = new PlayerDto();
            entity.gym_id = _client.id;
            //playerDto.FromPlayer(entity);
            return await _client.PostAsync("player", entity);

        }
        public async Task<int> CreateHandShake(Player entity, string uid)
        {
            HandShakeDto handShakeDto = new HandShakeDto();
            handShakeDto.gym_id = _client.id;
            handShakeDto.pid = entity.Id.ToString();
            handShakeDto.uid = uid;
            return await _client.PostAsync("handshake", handShakeDto);

        }
        public async Task<PlayerDto> Get(PlayerDto entity)
        {
            PlayerDto player = await _client.GetAsync<PlayerDto>($"player/{_client.id}/{entity.pid}");
            return player;
        }

        public async Task<Profile> GetProfile(string? uid)
        {
            ProfileDto profile = await _client.GetAsync<ProfileDto>($"profile/{uid}");
            return profile.ToProfile();
        }

        public async Task<int> Update(PlayerDto entity)
        {
            //PlayerDto playerDto = new PlayerDto();
            entity.gym_id = _client.id;
            //playerDto.FromPlayer(entity);
            return await _client.PutAsync("player", entity);
        }
    }
}
