using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;

namespace Unicepse.Core.Models.DailyActivity
{
    public class DailyPlayerReport : DomainObject
    {
        
        public DateTime Date { get; set; }
        public DateTime loginTime { get; set; }
        public DateTime logoutTime { get; set; }
        public virtual Player.Player? Player { get; set; }
        public virtual Subscription.Subscription? Subscription { get; set; }
        public bool IsTakenKey { get; set; }
        public bool IsLogged { get; set; }
    }
}
