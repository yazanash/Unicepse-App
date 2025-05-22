using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Payment;
using sub = Uniceps.Core.Models.Subscription;

namespace Uniceps.Core.Models.Player
{
    public class Player : Person
    {
        public Player()
        {
            Subscriptions = new HashSet<sub.Subscription>();
            Payments = new HashSet<PlayerPayment>();
        }
        public double Weight { get; set; }
        public double Hieght { get; set; }
        public DateTime SubscribeDate { get; set; }
        public DateTime SubscribeEndDate { get; set; }
        public bool IsTakenContainer { get; set; }
        public bool IsSubscribed { get; set; }
        [NotMapped]
        public double Balance { get; set; }
        public string? UID { get; set; }
        public DataStatus DataStatus { get; set; }
        public bool Conflicts(Player player)
        {
            if (player.FullName == FullName)
            {
                return true;
            }
            return false;
        }
        public virtual ICollection<sub.Subscription> Subscriptions { get; set; }
        public virtual ICollection<PlayerPayment> Payments { get; set; }

    }
}
