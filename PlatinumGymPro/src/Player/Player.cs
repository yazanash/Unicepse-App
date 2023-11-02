using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class Player:Person
    {
        public double Weight { get; set; }
        public double Hieght { get; set; }
        public DateTime SubscribeDate { get; set; }
        public DateTime SubscribeEndDate { get; set; }
        public bool IsTakenContainer { get; set; }
        public bool IsSubscribed { get; set; }
        public double Balance { get; set; }

        public bool Conflicts(Player player)
        {
           if(player.FullName == this.FullName)
            {
                return true;
            }
            return false;
        }
    }
}
