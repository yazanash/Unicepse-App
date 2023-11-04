using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models;
using PlatinumGym.Core.Models.Subscription;

namespace PlatinumGym.Core.Models.Payment
{
    public class PayReferance : DomainObject
    {
       
        public Player.Player? Player { get; set; }
        public PlayerTraining? PlayerTraining { get; set; }
        public double Value { get; set; }
        public DateTime RefDate { get; set; }
    }
}
