using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Services;
using Unicepse.Core.Models.DailyActivity;
using Unicepse.API.Models;

namespace Unicepse.API.Services
{
    public class AttendanceApiDataService : IApiDataService<AttendanceDto>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public AttendanceApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }
        public async Task<int> Create(AttendanceDto entity)
        {
            //AttendanceDto attendance = new AttendanceDto();
            //attendance.FromAttendance(entity);
            entity.gym_id = _client.id;
            return await _client.PostAsync("attendances", entity);
        }

        public async Task<AttendanceDto> Get(AttendanceDto entity)
        {
            AttendanceDto attendance = await _client.GetAsync<AttendanceDto>($"attendances/{_client.id}/{entity.pid}/{entity.aid}");
            return attendance;
        }

        public async Task<int> Update(AttendanceDto entity)
        {
            //AttendanceDto attendance = new AttendanceDto();
            //attendance.FromAttendance(entity);
            entity.gym_id = _client.id;
            return await _client.PutAsync("attendances", entity);
        }
    }
}
