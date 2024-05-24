using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;

namespace Unicepse.Core.Models.Expenses
{
    public class Expenses : DomainObject
    {
        public string? Description { get; set; }
        public double Value { get; set; }
        public DateTime date { get; set; }
        public bool isManager { get; set; }
        public virtual Employee.Employee? Recipient { get; set; }
    }
}
