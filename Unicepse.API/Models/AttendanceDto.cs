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

        public string? aid { get; set; }
        public string? pid { get; set; }
        public string? sid { get; set; }
        public string? gym_id { get; set; }
        public string? logout_time { get; set; }
        public string? login_time { get; set; }
        public string? date { get; set; }

        internal void FromAttendance(DailyPlayerReport entity)
        {
            aid = entity.Id.ToString();
            pid = entity.Player!.Id.ToString();
            sid = "";
            login_time = entity.loginTime.ToString("m:H");
            logout_time = entity.logoutTime.ToString("m:H");
            date = entity.Date.ToString("dd/MM/yyyy");

        }

        internal DailyPlayerReport ToAttendance()
        {
            DailyPlayerReport attendance = new DailyPlayerReport()
            {
                Id = Convert.ToInt32(aid),
                Player = new Core.Models.Player.Player() { Id = Convert.ToInt32( pid) },
                loginTime = DateTime.ParseExact(login_time!, "m:H", CultureInfo.InvariantCulture),
                logoutTime = DateTime.ParseExact(logout_time!, "m:H", CultureInfo.InvariantCulture),
                //Subscription = new Core.Models.Subscription.Subscription() { Id = sid },
                 Date = DateTime.ParseExact(date!, "dd/MM/yyyy", CultureInfo.InvariantCulture),


        };
           
            return attendance;
        }
    }
}
