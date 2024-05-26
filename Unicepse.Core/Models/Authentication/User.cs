using Unicepse.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Models.Authentication
{
    public class User : DomainObject
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public virtual Employee.Employee? Employee { get; set; }
        public bool Disable { get; set; }
    }
}
