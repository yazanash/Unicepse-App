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
    public class RoutineApiDataService : IApiDataService<RoutineDto>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public RoutineApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<int> Create(RoutineDto entity)
        {
            //RoutineDto routineDto = new RoutineDto();
            //routineDto.FromRoutine(entity);
            entity.gym_id = _client.id;
            return await _client.PostAsync("routines", entity);
        }

        public async Task<RoutineDto> Get(RoutineDto entity)
        {
            RoutineDto routineDto = await _client.GetAsync<RoutineDto>($"routines/{_client.id}/{entity.pid}/{entity.rid}");
            return routineDto;
        }

        public async Task<int> Update(RoutineDto entity)
        {
            //RoutineDto routineDto = new RoutineDto();
            //routineDto.FromRoutine(entity);
            entity.gym_id = _client.id;
            return await _client.PutAsync("routines", entity);
        }
    }
}
