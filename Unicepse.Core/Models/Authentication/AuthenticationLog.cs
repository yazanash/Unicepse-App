using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Models.Authentication
{
    public class AuthenticationLog : DomainObject
    {   
        public User? User { get; set; }
        public DateTime LoginDateTime { get;set ; }
        public bool status { get; set; }
    }
}
