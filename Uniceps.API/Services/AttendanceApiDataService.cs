using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Services;
using Uniceps.API;
using Uniceps.API.Models;

namespace Uniceps.API.Services
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
