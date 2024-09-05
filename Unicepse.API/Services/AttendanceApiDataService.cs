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
    public class AttendanceApiDataService : IApiDataService<DailyPlayerReport>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public AttendanceApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }
        public async Task<bool> Create(DailyPlayerReport entity)
        {
            AttendanceDto attendance = new AttendanceDto();
            attendance.FromAttendance(entity);
            attendance.gym_id = 18;
            return await _client.PostAsync("attendances", attendance);
        }

        public async Task<DailyPlayerReport> Get(DailyPlayerReport entity)
        {
            AttendanceDto attendance = await _client.GetAsync<AttendanceDto>($"attendances/18/{entity.Player!.Id}/{entity.Id}");
            return attendance.ToAttendance();
        }

        public async Task<bool> Update(DailyPlayerReport entity)
        {
            AttendanceDto attendance = new AttendanceDto();
            attendance.FromAttendance(entity);
            attendance.gym_id = 18;
            return await _client.PutAsync("attendances", attendance);
        }
    }
}
