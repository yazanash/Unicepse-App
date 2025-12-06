using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;

namespace Uniceps.Core.Models.Employee
{
    public class Credit : DomainObject
    {
        public virtual Employee? EmpPerson { get; set; }
        public double CreditValue { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
