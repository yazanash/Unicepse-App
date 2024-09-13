using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Models;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Core.Services;

namespace Unicepse.API.Services
{
    public class RoutineApiDataService : IApiDataService<PlayerRoutine>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public RoutineApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<int> Create(PlayerRoutine entity)
        {
            RoutineDto routineDto = new RoutineDto();
            routineDto.FromRoutine(entity);
            routineDto.gym_id = _client.id;
            return await _client.PostAsync("routines", routineDto);
        }

        public async Task<PlayerRoutine> Get(PlayerRoutine entity)
        {
            RoutineDto routineDto = await _client.GetAsync<RoutineDto>($"routines/{_client.id}/{entity.Player!.Id}/{entity.Id}");
            return routineDto.ToRoutine();
        }

        public async Task<int> Update(PlayerRoutine entity)
        {
            RoutineDto routineDto = new RoutineDto();
            routineDto.FromRoutine(entity);
            routineDto.gym_id = _client.id;
            return await _client.PutAsync("routines", routineDto);
        }
    }
}
