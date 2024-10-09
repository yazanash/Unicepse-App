using Unicepse.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;

namespace Unicepse.Core.Models.Authentication
{
    public class User : DomainObject
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Position { get; set; }
        public string? OwnerName { get; set; }
        public Roles Role { get; set; }
        public bool Disable { get; set; }
    }
}
