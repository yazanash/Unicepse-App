using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Sport;
using Sub = Uniceps.Core.Models.Subscription;

namespace Uniceps.Core.Models.Player
{
    public class Player : Person
    {
        public Player()
        {
            Subscriptions = new HashSet<Sub.Subscription>();
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

        public void MergeWith(Player player)
        {
            FullName = player.FullName;
            Phone = player.Phone;
            BirthDate = player.BirthDate;
            GenderMale = player.GenderMale;
            Weight = player.Weight;
            Hieght = player.Hieght;
            SubscribeDate = player.SubscribeDate;
            SubscribeEndDate = player.SubscribeEndDate;
            IsSubscribed = player.IsSubscribed;
            SyncId = player.SyncId;
        }

        public virtual ICollection<Sub.Subscription> Subscriptions { get; set; }
        public virtual ICollection<PlayerPayment> Payments { get; set; }

    }
}
