using PlatinumGym.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Core.Models.Employee
{
    public class Credit : DomainObject
    {
        public virtual Employee? EmpPerson { get; set; }
        public double CreditValue { get; set; }
        public DateTime Date { get; set; }
    }
}
