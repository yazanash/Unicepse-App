using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class Expenses : DomainObject
    {
        
        public string? Description { get; set; }
        public double Value { get; set; }
        public DateTime date { get; set; }
        public bool isManager { get; set; }
        public virtual Employee? Recipient { get; set; }
    }
}
