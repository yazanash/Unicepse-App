using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models
{
    public class OtherRevenue: DomainObject
    {
        public decimal Amount {get;set;}
        public string Service { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CustomerName {  get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
