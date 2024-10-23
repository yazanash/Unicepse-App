using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.DailyActivity;

namespace Unicepse.API.Models
{
    public class AttendanceDto
    {

        public int aid { get; set; }
        public string? pid { get; set; }
        public string? sid { get; set; }
        public string? gym_id { get; set; }
        public DateTime logout_time { get; set; }
        public DateTime login_time { get; set; }
        public DateTime date { get; set; }

        internal void FromAttendance(DailyPlayerReport entity)
        {
            aid = entity.Id;
            pid = entity.Player!.Id.ToString();
            sid = "";
            login_time = entity.loginTime.ToUniversalTime();
            logout_time = entity.logoutTime.ToUniversalTime();
            date = entity.Date.ToUniversalTime();

        }

        internal DailyPlayerReport ToAttendance()
        {
            DailyPlayerReport attendance = new DailyPlayerReport()
            {
                Id = aid,
                Player = new Core.Models.Player.Player() { Id = Convert.ToInt32(pid) },
                loginTime = login_time.ToLocalTime(),
                logoutTime = logout_time.ToLocalTime(),
                Date = date,

            };

            return attendance;
        }
    }
}
