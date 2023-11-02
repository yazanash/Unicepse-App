using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class DailyPlayerReport : DomainObject
    {
        
        public DateTime Date { get; set; }
        public DateTime loginTime { get; set; }
        public DateTime logoutTime { get; set; }
        public virtual Player? Player { get; set; }
        public bool IsTakenKey { get; set; }
    }
}
