using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;

namespace Unicepse.API.Models
{
    public class PlayerDto
    {
        public string? pid { get; set; }
        public string? name { get; set; }
        public string? phone_number { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public int date_of_birth { get; set; }
        public string? gender { get; set; }
        public double balance { get; set; }
        public string? gym_id { get; set; }
        public string? subs_date { get; set; }
        public string? subs_end_date { get; set; }

        public void FromPlayer(Player player)
        {

            pid = player.Id.ToString();
            name = player.FullName;
            phone_number = player.Phone;
            balance = player.Balance;
            date_of_birth = player.BirthDate;
            gender = player.GenderMale.ToString();
            subs_date = player.SubscribeDate.ToShortDateString();
            subs_end_date = player.SubscribeEndDate.ToShortDateString();

        }
        public Player ToPlayer()
        {
            Player player = new Player()
            {
                Id = Convert.ToInt32( pid),
                FullName = name,
                Phone = phone_number,
                Balance = balance,
                BirthDate = date_of_birth,
                GenderMale = Convert.ToBoolean(gender),
                SubscribeDate = Convert.ToDateTime(subs_date),
                SubscribeEndDate = Convert.ToDateTime(subs_end_date),
            };

            return player;
        }
    }
}
