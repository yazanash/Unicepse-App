using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class PayReferance : DomainObject
    {
       
        public Player? Player { get; set; }
        public PlayerTraining? PlayerTraining { get; set; }
        public double Value { get; set; }
        public DateTime RefDate { get; set; }
    }
}
