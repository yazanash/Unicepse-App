using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models;

namespace Unicepse.API.Models
{
    public class LicenseDto
    {
        public string? gym_id { get; set; }
        public string? plan { get; set; }
        public DateTime subscribe_date { get; set; }
        public DateTime? subscribe_end_date { get; set; }
        public string? token { get; set; }

        internal License ToLicense()
        {
            License license = new License()
            {
                GymId = gym_id,
                Plan = plan,
                SubscribeDate = subscribe_date,
                SubscribeEndDate = subscribe_end_date,
                Token = token


            };
            return license;
        }
        internal void FromLicense(License entity)
        {
            gym_id= entity.GymId;
            plan = entity.Plan;
            subscribe_date = entity.SubscribeDate;
            subscribe_end_date = entity.SubscribeEndDate;
            token = entity.Token;
        }
    }
}
