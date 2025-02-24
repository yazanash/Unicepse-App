using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Subscription;

namespace Unicepse.Core.Models.Player
{
    public class Player : Person
    {
        public Player()
        {
            Subscriptions = new HashSet<Subscription.Subscription>();
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
            if (player.FullName == this.FullName)
            {
                return true;
            }
            return false;
        }
        public virtual ICollection<Subscription.Subscription> Subscriptions { get; set; }
        public virtual ICollection<PlayerPayment> Payments { get; set; }

    }
}
